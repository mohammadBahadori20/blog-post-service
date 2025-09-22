using System.ComponentModel.DataAnnotations;

namespace BlogpostService.Application.DTOs;

public class BlogpostDto
{
    [MaxLength(36)]
    public Guid? BlogPostId { set; get; }

    [MaxLength(length: 30, ErrorMessage = "The maximum length of the title must be less than 30")]
    public string? Title { get; set; } = string.Empty;

    [MaxLength(length: 100, ErrorMessage = "The maximum length of the description must be less than 100")]
    public string? Description { get; set; } = string.Empty;

    public Uri? Comments { get; set; }
}