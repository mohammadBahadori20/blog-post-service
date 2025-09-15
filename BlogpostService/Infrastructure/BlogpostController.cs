using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using BlogpostService.Application.DTOs;
using Microsoft.AspNetCore.Mvc;


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
        [FromRoute] [Required(ErrorMessage = "the blogpost ID should be supplied")]
        string blogpostId)
    {
        List<CommentDto>? commentDto = await _blogpostService.GetBlogpostComments(blogpostId);

        if (commentDto is null)
        {
            return BadRequest(new ApiErrorResponse()
            {
                StatusCode = 400,
                Title = "Username not found.",
                Detail = $"The blogpost with {blogpostId} cannot be found"
            });
        }

        return commentDto;
    }


    //user must be authenticated here.
    [HttpPost]
    public async Task<ActionResult<BlogpostDto>> AddNewBlogpost([FromBody] BlogpostDto blogpostDto)
    {
        await _blogpostService.AddNewBlogpostForAuthor(blogpostDto,
            User.Claims.FirstOrDefault(c => c.Type == "sub")!.Value);
        return blogpostDto;
    }


    //user must be authenticated here.
    [HttpPost("{blogpostId}/comment")]
    public async Task<ActionResult<CommentDto>> AddNewCommentForBlogpost([FromBody] CommentDto commentDto,
        [FromRoute] [Required] string blogpostId)
    {
        string authorId = User.Claims.FirstOrDefault(c => c.Type == "sub")!.Value; 
        CommentDto? responseComment =  await _blogpostService.AddNewCommentForBlogpost(commentDto, blogpostId, authorId);
        if (responseComment is null)
        {
            return BadRequest(new ApiErrorResponse()
            {
                StatusCode = 400,
                Title = "Username not found.",
                Detail = $"The blogpost with {blogpostId} cannot be found"
            });
        }

        return responseComment;
    }
}