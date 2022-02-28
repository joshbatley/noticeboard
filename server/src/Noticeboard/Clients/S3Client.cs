using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using Noticeboard.Core.Configuration;
using ILogger = Serilog.ILogger;


namespace Noticeboard.Core.Clients;

public interface IS3Client
{
    Task<ListObjectsResponse> ListBucketsAsync();
    Task UploadFile(IFormFile file);
}

public class S3Client : IS3Client
{
    private readonly IAmazonS3 _amazonS3;
    private readonly ILogger _logger;
    private readonly string _bucket;

    public S3Client(IAmazonS3 amazonS3, ILogger logger, IOptions<S3Options> s3Options)
    {
        _amazonS3 = amazonS3;
        _logger = logger;
        _bucket = s3Options.Value.bucketName;
    }

    public async Task<ListObjectsResponse> ListBucketsAsync()
    {
        return await _amazonS3.ListObjectsAsync(_bucket);
    }

    public async Task UploadFile(IFormFile file)
    {
        var request = new PutObjectRequest
        {
            InputStream = file.OpenReadStream(),
            Key = file.FileName,
            BucketName = _bucket,
            ContentType = file.ContentType
        };
        await _amazonS3.PutObjectAsync(request);
    }
}