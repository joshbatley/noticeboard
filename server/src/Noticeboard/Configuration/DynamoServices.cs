using Amazon.DynamoDBv2;
using Noticeboard.Core.Clients;

namespace Noticeboard.Core.Configuration;

public static class DynamoServices
{
    public static void AddDynamoDB(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddAWSService<IAmazonDynamoDB>();
        services.Configure<DynamoOptions>(configuration.GetSection("DynamoDB"));
        services.AddSingleton<IDynamoClient, DynamoClient>();
    }
}