namespace BlogpostService.Domain;

public class Comment
{
    public Guid? BlogPostId { get; set; }
    public Guid? CommentId { get; set; }
    public Guid? AuthorId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
    public List<Comment> Replies { get; set; } = new();
}