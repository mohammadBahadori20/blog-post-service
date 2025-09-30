using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using BlogpostService.Application;
using BlogpostService.Application.DTOs;
using BlogpostService.fault_tolerance_policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace BlogpostService.Infrastructure;

[ApiController]
[Route("/api/blogpost")]
public class BlogpostController : ControllerBase
{
    private readonly IBlogpostService _blogpostService;
    private readonly IDistributedRedisCache _cache;

    public BlogpostController(IBlogpostService blogpostService, IDistributedRedisCache cache)
    {
        _blogpostService = blogpostService;
        _cache = cache;
    }

    [HttpGet("{blogpostId:guid}")]
    public async Task<ActionResult<BlogpostDto>> GetBlogpost(Guid blogpostId, CancellationToken cancellationToken = default)
    {
        string key = $"blogpost:{blogpostId}";
        string? jsonBlogpost = await _cache.GetStringAsync(key,cancellationToken);

        if (jsonBlogpost is not null)
        {
            BlogpostDto blogpost = JsonSerializer.Deserialize<BlogpostDto>(jsonBlogpost)!;
            return Ok(blogpost);
        }

        BlogpostDto? blogpostDto = await _blogpostService.GetBlogpost(blogpostId);
        jsonBlogpost = JsonSerializer.Serialize(blogpostDto);

        await _cache.SetStringAsync(key,jsonBlogpost,cancellationToken);
        
        if (blogpostDto is null)
        {
            return NotFound(new ApiErrorResponse()
            {
                StatusCode = 404,
                Title = "blog post not found.",
                Detail = $"The blogpost with ID: {blogpostId} cannot be found"
            });
        }

        string blogpostDtoJson = JsonSerializer.Serialize(blogpostDto);

        await _cache.SetStringAsync(key, blogpostDtoJson, cancellationToken);

        return Ok(blogpostDto);
    }

    [HttpGet("{blogpostId:guid}/comments")]
    public async Task<ActionResult<List<CommentDto>>> GetBlogpostComments(
        [FromRoute] Guid blogpostId, [FromQuery] int pageSize = 2, [FromQuery] int page = 1,
        CancellationToken cancellationToken = default)
    {
        string key = $"blogpost:{blogpostId}:comments:page:{page}:pageSize:{pageSize}";

        string? jsonComments = await _cache.GetStringAsync(key,cancellationToken);

        if (jsonComments is not null)
        {
            List<CommentDto> comments = JsonSerializer.Deserialize<List<CommentDto>>(jsonComments);
            return Ok(comments);
        }
        
        
        List<CommentDto>? commentDtos =
            await _blogpostService.GetBlogpostCommentsById(blogpostId, pageSize, page);

        jsonComments = JsonSerializer.Serialize(commentDtos);
        
        await _cache.SetStringAsync(key, jsonComments,cancellationToken);

        if (commentDtos is null)
        {
            return NotFound(new ApiErrorResponse()
            {
                StatusCode = 404,
                Title = "blog post not found.",
                Detail = $"The blogpost with ID: {blogpostId} cannot be found"
            });
        }
        
        return Ok(commentDtos);
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