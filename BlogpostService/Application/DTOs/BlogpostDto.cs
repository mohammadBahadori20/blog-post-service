namespace BlogpostService.Application.DTOs;

public class BlogpostDto
{
    public Guid BlogPostId { set; get; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public DateTime? PublishedAt { get; set; }
    public Uri? Comments { get; set; }
}