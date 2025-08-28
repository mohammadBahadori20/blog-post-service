namespace BlogpostService.Application.DTOs;

public class AuthorDto
{
    public Guid? AuthorId { get; set; }
    public string? Username { get; set; }
    public Uri? AuthorBlogposts { get; set; }
    
}