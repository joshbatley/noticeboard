using MediatR;
using Noticeboard.Core.Clients;
using Noticeboard.Core.Models;
using Noticeboard.Core.Models.DynamoDB;
using Noticeboard.Core.Models.NoticeItem;
using Noticeboard.Core.Models.Responses;
using OneOf;
using OneOf.Types;
using ILogger = Serilog.ILogger;

namespace Noticeboard.Core.Handlers;

public class NoticeboardHandler
{
    public class Query : IRequest<NoticeboardResult>
    {
        public NoticeRequest Item;

        public Query(NoticeRequest item) => 
            (Item) = (item);
    }

    public class NoticeboardResult : OneOfBase<NoticeboardResponse, Error>
    {
        public NoticeboardResult(NoticeboardResponse response) : base(response) { }
        public NoticeboardResult(Error error) : base(error) { }
    }

    public class Handler : IRequestHandler<Query, NoticeboardResult>
    {
        private readonly ILogger _logger;
        private readonly IS3Client _s3Client;
        private readonly IDynamoClient _dynamoClient;

        public Handler(ILogger logger, IS3Client s3Client, IDynamoClient dynamoClient)
        {
            _logger = logger;
            _s3Client = s3Client;
            _dynamoClient = dynamoClient;
        }
        
        public async Task<NoticeboardResult> Handle(Query request, CancellationToken cancellationToken)
        {
            var file = request.Item.files.FirstOrDefault();
            var newDocument = new NoticeDocument
            {
                UserId = "my_user_id",
                Type = NoticeType.File,
                Name = file.FileName,
                Tags = new string[]{ },
                Archived = false,
                TTL = 0,
            };
            await _s3Client.UploadFile(file, newDocument.Id);
            await _dynamoClient.CreateItem(newDocument);
            var items = await _s3Client.ListBucketsAsync();
            return new NoticeboardResult(new NoticeboardResponse{items = items});
        }

        // private Task AddFileToS3(IFormFile files, string fileName)
        // {
        //     
        // }

    //     private async Task<string> AddRecord(string file)
    //     {
    //         var doc = new NoticeDocument()
    //         return doc.Id;
    //     }
     }
}