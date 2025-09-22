namespace BlogpostService.Domain;

public class Comment
{
    public Guid BlogPostId { get; set; } = Guid.Empty;
    
    public Guid CommentId { get; set; } = Guid.Empty;
    
    public Guid? AuthorId { get; set; } = Guid.Empty;
    
    public Guid? ParentId { get; set; }
    
    public string Content { get; set; } = string.Empty;
    
    public DateTime? CreatedAt { get; set; }
    
    public Blogpost? Blogpost { get; set; }       
    
    public Comment? Parent { get; set; }         
    
    public List<Comment> Replies { get; set; } = new(); 
}