using MediatR;
using Noticeboard.Core.Clients;
using Noticeboard.Core.Models;
using Noticeboard.Core.Models.Responses;
using OneOf;
using OneOf.Types;
using ILogger = Serilog.ILogger;

namespace Noticeboard.Core.Handlers;

public class NoticeboardHandler
{
    public class Query : IRequest<NoticeboardResult>
    {
        public NoticeboardItem Item;

        public Query(NoticeboardItem item) => 
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

        public Handler(ILogger logger, IS3Client s3Client)
        {
            _logger = logger;
            _s3Client = s3Client;
        }
        
        public async Task<NoticeboardResult> Handle(Query request, CancellationToken cancellationToken)
        {
            foreach (var itemFile in request.Item.files)
            {
                await _s3Client.UploadFile(itemFile);
            }
            var items = await _s3Client.ListBucketsAsync();
            return new NoticeboardResult(new NoticeboardResponse{items = items});
        }
    }
}