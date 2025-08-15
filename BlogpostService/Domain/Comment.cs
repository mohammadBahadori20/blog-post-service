namespace BlogpostService.Domain;

public class Comment
{
    public string? Content { get; set; }
    public DateTime? CreatedAt { get; set; }
    public Author Author { get; set; }
    public List<Comment> Replies { get; set; } = new();
}