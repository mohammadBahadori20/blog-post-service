using BlogpostService.Domain;

namespace BlogpostService.Application.DTOs;

public class CommentDto
{
    public string? AuthorId { get; set; }
    public string? Content { get; set; }
    public DateTime? CreatedAt { get; set; }
    public List<Comment> Replies { get; set; } = new();
}