using System.Runtime.Serialization;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Newtonsoft.Json;
using Noticeboard.Core.Models.NoticeItem;

namespace Noticeboard.Core.Models.DynamoDB;

public class NoticeDocument
{
    public string Id => GenerateBoardId();
    public string GeneratedDate => DateTime.UtcNow.ToString("s");
    public string UserId { get; set; }
    public string FileUrl => ConstructFileUrl(this);
    public NoticeType Type { get; set; }
    public string Name { get; set;  }
    public IReadOnlyList<string> Tags { get; set; }
    public bool Archived { get; set;  }
    public int TTL { get; set; }
    public string Notes { get; set; }
    public string Priority { get; set; }

    // public NoticeDocument(string userId, string? fileUrl, NoticeType type, string? name, 
    //     IReadOnlyList<string>? tags, bool? archived, int? ttl, string? notes, string? priority)
    // {
    //     var newId = GenerateBoardId();
    //     Id = GenerateBoardId();
    //     UserId = userId;
    //     FileUrl = 
    //     Type = type.ToString();
    //     Name = name;
    //     Tags = tags;
    //     Archived = archived ?? false;
    //     TTL = ttl ?? 0;
    //     Notes = notes;
    //     Priority = priority;
    // }

    private static string ConstructFileUrl(NoticeDocument doc) => doc.Type switch
    {
        NoticeType.File => $"{doc.UserId}/{doc.Id}",
        NoticeType.Image => $"{doc.UserId}/{doc.Id}",
        NoticeType.Url => doc.FileUrl,
        NoticeType.Text => string.Empty,
        _ => throw new ArgumentOutOfRangeException(nameof(doc.Type), doc.Type, "NoticeType is not implemented")
    };

    private static string GenerateBoardId() => $"ntc_{Guid.NewGuid()}";
    
    public Dictionary<string, AttributeValue> ToAttributeMap() => Document.FromJson(JsonConvert.SerializeObject(this)).ToAttributeMap();
}