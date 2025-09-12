using System.Collections;

namespace BlogpostService.Domain;

public class Blogpost
{
    public string? BlogPostId { set; get; }
    public string? AuthorId { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public DateTime? PublishedAt { get; set; }
    public List<Comment> Comments { get; set; } = new();
}