using BlogpostService.Domain;

namespace BlogpostService.Application.DTOs;

public class CommentDto
{
    public string Username { get; set; }
    public string? Content { get; set; }
    public DateTime? CreatedAt { get; set; }
    public List<Comment> Replies { get; set; }
}