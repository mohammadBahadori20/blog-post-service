using System.ComponentModel.DataAnnotations;
using BlogpostService.Domain;

namespace BlogpostService.Application.DTOs;

public class CommentDto
{
    public string? AuthorId { get; set; }
    [Required] [MaxLength(length: 50)] public string? Content { get; set; }
    public DateTime? CreatedAt { get; set; }
    public List<Comment> Replies { get; set; } = new();
}