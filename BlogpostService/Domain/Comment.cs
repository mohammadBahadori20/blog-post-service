namespace BlogpostService.Domain;

public class Comment
{
    public string? BlogPostId { get; set; }
    public string? CommentId { get; set; }
    public string? AuthorId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
    public List<Comment> Replies { get; set; } = new();
}