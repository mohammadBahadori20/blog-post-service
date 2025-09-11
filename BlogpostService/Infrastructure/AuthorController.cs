using System.ComponentModel.DataAnnotations;
using BlogpostService.Application;
using BlogpostService.Application.DTOs;
using BlogpostService.Domain;
using Microsoft.AspNetCore.Mvc;


namespace BlogpostService.Infrastructure;

[ApiController]
[Route("/api/[controller]")]
public class AuthorController : ControllerBase
{
    private readonly IAuthorService _authorService;
    private readonly IConfiguration _configuration;

    public AuthorController(IAuthorService authorService, IConfiguration configuration)
    {
        _authorService = authorService;
        _configuration = configuration;
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<AuthorDto>> GetAuthor([FromRoute][Required] string username)
    {

        Author? author = await _authorService.GetAuthorByUsername(username);
        if (author == null)
        {
            return NotFound(new ApiErrorResponse()
            {
                StatusCode = 404,
                Title = "Author not found.",
                Detail = "The author could not be found with specified username"
            });
        }

        string host = _configuration["AuthorMicroservice_HOST"]!;
        string port = _configuration["AuthorMicroservice_Port"]!;
        return new AuthorDto()
        {
            AuthorId = author.AuthorId,
            Username = author.Username,
            AuthorBlogposts = new Uri($"http://{host}:{port}/api/Author/blogposts/{username}")
        };
    }


    [HttpGet("/blogposts/{username}")]
    public async Task<ActionResult<List<BlogpostDto>>> GetAuthorBlogposts([FromRoute][Required] string username)
    {
        List<BlogpostDto>? blogpostDtos = await _authorService.GetAuthorBlogpostsByUsername(username);

        if (blogpostDtos is null)
        {
            return NotFound(new ApiErrorResponse()
            {
                StatusCode = 404,
                Title = "The Author is not found",
                Detail = $"The author with username: {username} could not be found"
            });
        }

        return blogpostDtos;
    }

    [HttpPost]
    public async Task<ActionResult<AuthorDto>> AddNewAuthor([FromBody] AuthorDto authorDto)
    {
        await _authorService.AddNewAuthor(authorDto);
        return authorDto;
    }
    
    
    
}