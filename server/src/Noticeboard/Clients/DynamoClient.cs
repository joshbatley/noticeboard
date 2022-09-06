using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Noticeboard.Core.Configuration;
using Noticeboard.Core.Models.DynamoDB;
using OneOf.Types;
using ILogger = Serilog.ILogger;

namespace Noticeboard.Core.Clients;

public interface IDynamoClient
{
    public Task<NoticeDocumentResponse> GetItemById(string id);
    public Task CreateItem(NoticeDocument noticeDocument);
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

    public async Task<NoticeDocumentResponse> GetItemById(string id)
    {
        var getRequest = new GetItemRequest
        {
            TableName = _tableName,
            Key = new Dictionary<string, AttributeValue>
            {
                { "Id", new AttributeValue { S = id } }
            }
        };

        var result = await _dynamoDb.GetItemAsync(getRequest);
        var doc = Document.FromAttributeMap(result.Item);
        return JsonConvert.DeserializeObject<NoticeDocumentResponse>(doc.ToJson());
    }

    public async Task CreateItem(NoticeDocument noticeDocument)
    {
        var t = await _dynamoDb.ListTablesAsync();
        var putRequest = new PutItemRequest
        {
            Item = noticeDocument.ToAttributeMap(),
            TableName = _tableName,
        };
        await _dynamoDb.PutItemAsync(putRequest);
    }
}