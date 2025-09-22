using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace BlogpostService.Domain;

public class Blogpost
{
    public Guid BlogPostId { set; get; } = Guid.Empty;

    public Guid? AuthorId { get; set; } = Guid.Empty;

    public string? Title { get; set; }

    public string? Description { get; set; } = string.Empty;

    public DateTime? PublishedAt { get; set; }

    public List<Comment> Comments { get; set; } = new();

    public void AddNewComment(Comment comment)
    {
        Comments.Add(comment);
    }
}