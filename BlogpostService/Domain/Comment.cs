namespace BlogpostService.Domain;

public class Comment
{
    public Guid? BlogPostId { get; set; }
    public string Content { get; set; }
    public DateTime? CreatedAt { get; set; }
    public List<Comment> Replies { get; set; } = new();
}