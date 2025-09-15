using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace BlogpostService.Domain;

public class Blogpost
{
    [MaxLength(36)]
    public string? BlogPostId { set; get; }
    
    [MaxLength(36)]
    public string? AuthorId { get; set; }
    
    [MaxLength(length: 100, ErrorMessage = "The maximum length of the title must be less than 30")]
    public string? Title { get; set; }
    
    [MaxLength(length: 100, ErrorMessage = "The maximum length of the description must be less than 100")]
    public string? Description { get; set; } = string.Empty;
    
    public DateTime? PublishedAt { get; set; }
    
    public List<Comment> Comments { get; set; } = new();

    public void AddNewComment(Comment comment)
    {
        Comments.Add(comment);
    }
}