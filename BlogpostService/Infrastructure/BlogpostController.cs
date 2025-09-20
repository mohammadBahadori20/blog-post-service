using System.ComponentModel.DataAnnotations;
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
        List<CommentDto>? commentDto = await _blogpostService.GetBlogpostCommentsById(blogpostId);

        if (commentDto is null)
        {
            return NotFound(new ApiErrorResponse()
            {
                StatusCode = 404,
                Title = "Username not found.",
                Detail = $"The blogpost with ID: {blogpostId} cannot be found"
            });
        }

        return commentDto;
    }


    //user must be authenticated here.
    [HttpPost]
    public async Task<ActionResult<BlogpostDto>> AddNewBlogpost([FromBody] BlogpostDto blogpostDto)
    {
        await _blogpostService.PublishBlogpost(blogpostDto,
            User.Claims.FirstOrDefault(c => c.Type == "sub")!.Value);
        return blogpostDto;
    }


    //user must be authenticated here.
    [HttpPost("{blogpostId}/comments")]
    public async Task<ActionResult<CommentDto>> AddNewCommentForBlogpost([FromBody] CommentDto commentDto,
        [FromRoute] [Required] string blogpostId)
    {
        string authorId = User.Claims.FirstOrDefault(c => c.Type == "sub")!.Value;
        CommentDto? responseComment = await _blogpostService.AddCommentForBlogpost(commentDto, blogpostId, authorId);
        if (responseComment is null)
        {
            return NotFound(new ApiErrorResponse()
            {
                StatusCode = 404,
                Title = "blogpost not found.",
                Detail = $"The blogpost with ID: {blogpostId} cannot be found"
            });
        }

        return responseComment;
    }

    //user must be authenticated here.
    [HttpDelete("{blogpostId}")]
    public async Task<IActionResult> DeleteBlogpost([FromRoute] string blogpostId)
    {
        bool state = await _blogpostService.DeleteBlogpost(blogpostId);
        if (state)
        {
            return NotFound(new ApiErrorResponse()
            {
                StatusCode = 404,
                Title = "blogpost not found.",
                Detail = $"The blogpost with ID: {blogpostId} cannot be found"
            });
        }

        return NoContent();
    }

    [HttpPut("{blogpostId}")]
    public async Task<ActionResult<UpdatedBlogpostDto>> UpdateBlogpost(
        [Required(ErrorMessage = "The details for updating the blogpost are needed.")]
        UpdatedBlogpostDto updatedBlogpost,
        [FromRoute] string blogpostId)
    {
        BlogpostDto? blogpostDto = await _blogpostService.UpdateBlogpost(updatedBlogpost, blogpostId);
        if (blogpostDto == null)
        {
            return NotFound(new ApiErrorResponse()
            {
                StatusCode = 404,
                Title = "blogpost not found.",
                Detail = $"The blogpost with ID: {blogpostId} cannot be found"
            });
        }

        return Ok(blogpostDto);
    }
    
}