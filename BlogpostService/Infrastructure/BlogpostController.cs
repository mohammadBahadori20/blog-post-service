using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using BlogpostService.Application;
using BlogpostService.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;


namespace BlogpostService.Infrastructure;

[ApiController]
[Route("/api/blogpost")]
public class BlogpostController : ControllerBase
{
    private readonly IBlogpostService _blogpostService;
    private readonly IDistributedCache _cache;

    public BlogpostController(IBlogpostService blogpostService, IDistributedCache cache)
    {
        _blogpostService = blogpostService;
        _cache = cache;
    }

    [HttpGet("{blogpostId:guid}")]
    public async Task<ActionResult<BlogpostDto>> GetBlogpost(Guid blogpostId)
    {
        string key = $"blogpost:{blogpostId}";
        string? cachedBlogpost = await _cache.GetStringAsync(key);
        
        if (cachedBlogpost is not null)
        {
            BlogpostDto blogpost = JsonSerializer.Deserialize<BlogpostDto>(cachedBlogpost)!;
            return Ok(blogpost);
        }

        BlogpostDto? blogpostDto = await _blogpostService.GetBlogpost(blogpostId);
        if (blogpostDto is null)
        {
            return NotFound(new ApiErrorResponse()
            {
                StatusCode = 404,
                Title = "blog post not found.",
                Detail = $"The blogpost with ID: {blogpostId} cannot be found"
            });
        }

        return Ok(blogpostDto);

    }

    [HttpGet("{blogpostId:guid}/comments")]
    public async Task<ActionResult<List<CommentDto>>> GetBlogpostComments(
        [FromRoute] Guid blogpostId, [FromQuery] int pageSize = 2, [FromQuery] int page = 1)
    {
        List<CommentDto>? commentDtos =
            await _blogpostService.GetBlogpostCommentsById(blogpostId, pageSize, page);

        if (commentDtos is null)
        {
            return NotFound(new ApiErrorResponse()
            {
                StatusCode = 404,
                Title = "blog post not found.",
                Detail = $"The blogpost with ID: {blogpostId} cannot be found"
            });
        }

        return commentDtos;
    }


    //user must be authenticated here.
    [HttpPost]
    public async Task<ActionResult<BlogpostDto>> AddNewBlogpost([FromBody] BlogpostDto blogpostDto)
    {
        await _blogpostService.PublishBlogpost(blogpostDto,
            Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "sub")!.Value));
        return blogpostDto;
    }


    [Authorize]
    [HttpPost("{blogpostId}/comments")]
    public async Task<ActionResult<CommentDto>> AddNewCommentForBlogpost([FromBody] CommentDto commentDto,
        [FromRoute] [Required] Guid blogpostId)
    {
        string authorId = User.Claims.FirstOrDefault(c => c.Type == "sub")!.Value;
        CommentDto? responseComment =
            await _blogpostService.AddCommentForBlogpost(commentDto, blogpostId, Guid.Parse(authorId));
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

    [Authorize]
    [HttpDelete("{blogpostId}")]
    public async Task<IActionResult> DeleteBlogpost([FromRoute] Guid blogpostId)
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

    [Authorize]
    [HttpPut("{blogpostId}")]
    public async Task<ActionResult<UpdatedBlogpostDto>> UpdateBlogpost(
        [Required(ErrorMessage = "details for updating the blogpost are needed.")]
        UpdatedBlogpostDto updatedBlogpost,
        [FromRoute] Guid blogpostId)
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