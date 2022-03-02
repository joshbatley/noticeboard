using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Options;
using Noticeboard.Core.Configuration;
using ILogger = Serilog.ILogger;

namespace Noticeboard.Core.Clients;

public interface IDynamoClient
{
    public Task<ListTablesResponse> Get();
}

public class DynamoClient : IDynamoClient
{
    private readonly IAmazonDynamoDB _dynamoDb;
    private readonly ILogger _logger;
    private readonly string _tableName;
    public DynamoClient(IAmazonDynamoDB dynamoDb, ILogger logger, IOptions<DynamoOptions> dynamoOptions)
    {
        _dynamoDb = dynamoDb;
        _logger = logger;
        _tableName = dynamoOptions.Value.BoardsTableName;
    }

    public async Task<ListTablesResponse> Get()
    {
        return await _dynamoDb.ListTablesAsync();
    }
    
    public async Task PutObject()
    {
        var putRequest = new PutItemRequest
        {
            Item = null,
            TableName = null
        };
        await _dynamoDb.PutItemAsync(putRequest);
    }
}