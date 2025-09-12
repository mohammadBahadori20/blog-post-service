using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using BlogpostService.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BlogpostService.Infrastructure;

[ApiController]
[Route("/api/blogpost")]
public class BlogpostController : ControllerBase
{
    private readonly IBlogpostService _blogpostService;

    public BlogpostController(IBlogpostService blogpostService)
    {
        _blogpostService = blogpostService;
    }


    [HttpGet("{blogpostId}")]
    public async Task<ActionResult<List<CommentDto>>> GetBlogpostComments(
        [FromRoute][Required(ErrorMessage = "the blogpost ID should be supplied")] string blogpostId)
    {
        List<CommentDto>? commentDto = await _blogpostService.GetBlogpostComments(blogpostId);

        if (commentDto is null)
        {
            return BadRequest(new ApiErrorResponse()
            {
                StatusCode = 404,
                Title = "Username not found.",
                Detail = $"The blogpost with {blogpostId} cannot be found"
            });
        }

        return commentDto;
    }


    [HttpPost]
    public async Task<ActionResult<BlogpostDto>> AddNewBlogpost([FromBody] BlogpostDto blogpostDto,
        [FromQuery] string authorUsername)
    {
        await _blogpostService.AddNewBlogpostForAuthor(blogpostDto, authorUsername);
        return blogpostDto;
    }

    // [HttpPost("{blogpostId}/comment")]
    // public async Task<ActionResult<CommentDto>> AddNewCommentForBlogpost([FromBody] CommentDto commentDto,
    //     [FromRoute] [Required] string blogpostId)
    // {
    // }
}