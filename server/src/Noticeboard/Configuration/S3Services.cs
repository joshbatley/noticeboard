using Amazon.S3;
using Noticeboard.Core.Clients;

namespace Noticeboard.Core.Configuration;

public static class S3Services
{
    public static void AddS3Services(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddAWSService<IAmazonS3>();
        services.Configure<S3Options>(configuration.GetSection("S3"));
        services.AddSingleton<IS3Client, S3Client>();
    }
}