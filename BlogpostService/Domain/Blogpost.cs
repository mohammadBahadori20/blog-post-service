using System.Collections;

namespace BlogpostService.Domain;

public class Blogpost
{
    public Guid BlogPostId { set; get; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public DateTime? PublishedAt { get; set; }
    public List<Comment> Comments { get; set; } = new();
}