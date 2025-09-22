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

    public long Likes { get; set; } = 0;
    
    public long DisLikes { get; set; } = 0;

    public List<Comment> Comments { get; set; } = new();

    public void AddNewComment(Comment comment)
    {
        Comments.Add(comment);
    }

    public void IncrementLike()
    {
        Likes++;
    }
    
    public void IncrementDisLike()
    {
        DisLikes++;
    }
    
    public void DecrementLike()
    {
        if (Likes > 0)
        {
            --Likes;
        }
    }
    
    public void DecrementDisLike()
    {
        if (DisLikes > 0)
        {
            --DisLikes;
        }
    }
}