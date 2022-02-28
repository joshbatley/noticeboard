using Amazon.S3.Model;

namespace Noticeboard.Core.Models.Responses;

public class NoticeboardResponse
{
    public ListObjectsResponse items { get; set; }
}