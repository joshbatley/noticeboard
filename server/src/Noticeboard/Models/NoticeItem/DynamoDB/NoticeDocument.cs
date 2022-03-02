using System.Runtime.Serialization;

namespace Noticeboard.Core.Models.DynamoDB;

public class NoticeDocument
{
    [DataMember(Name = "pk")]
    public string UserId { get; }
    
    [DataMember(Name = "sk")]
    public string Id { get; }
    
    public string FileUrl { get; }
    public string Type { get; }
    public string Name { get; }
    public string PublishedDateTime { get; }
    public IReadOnlyList<string> Tags { get; }
    public bool Archived { get; }
    public int TTL { get; }
    public string Notes { get; }
    public string Priority { get; }

    private NoticeDocument(string id, string userId, string fileUrl, string type, string name,
        string publishedDateTime, IReadOnlyList<string> tags, bool archived, int ttl, string notes, string priority)
    {
        Id = id;
        UserId = userId;
        FileUrl = fileUrl;
        Type = type;
        Name = name;
        PublishedDateTime = publishedDateTime;
        Tags = tags;
        Archived = archived;
        TTL = ttl;
        Notes = notes;
        Priority = priority;
    }

    public static NoticeDocument Create(string fileUrl, string type, string name, string publishedDateTime, IReadOnlyList<string> tags, bool archived, int ttl, string notes, string priority)
    {
        return new NoticeDocument("id", "user",fileUrl, type, name, publishedDateTime, tags, archived, ttl, notes, priority);
    }
    
    private static string GenerateBoardId() => $"ntc_{Guid.NewGuid()}";

}