using Noticeboard.Core.Models.NoticeItem;

namespace Noticeboard.Core.Models.DynamoDB;

public class NoticeDocumentResponse
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public DateTime GeneratedDate { get; set; }
    public string FileUrl { get; set; }
    public NoticeType Type { get; set; }
    public string Name { get; set; }
    public IReadOnlyList<string> Tags { get; set; }
    public bool Archived { get; set; }
    public int TTL { get; set; }
    public string Notes { get; set; }
    public NoticePriority Priority { get; set; }
}