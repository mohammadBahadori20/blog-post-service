using System.Text.Json;
using BlogpostService.Application;
using BlogpostService.Application.DTOs;
using BlogpostService.fault_tolerance_policies;
using Microsoft.AspNetCore.Mvc;

namespace BlogpostService.Infrastructure;

[ApiController]
[Route("/api/[controller]")]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;
    private readonly ILogger<CommentController> _logger;
    private readonly IDistributedRedisCache _cache;

    public CommentController(ICommentService commentService, ILogger<CommentController> logger,
        IDistributedRedisCache cache)
    {
        _commentService = commentService;
        _logger = logger;
        _cache = cache;
    }

    [HttpGet("/{commentId}/replies")]
    public async Task<ActionResult<ReplyDto>> GetReplies(Guid commentId, int pageSize = 5, int page = 1,
        CancellationToken cancellationToken = default)
    {
        string key = $"comment:{commentId}:page:{page}:pageSize:{pageSize}";

        string? jsonReplies = await _cache.GetStringAsync(key, cancellationToken);

        if (jsonReplies is not null)
        {
            ReplyDto reply = JsonSerializer.Deserialize<ReplyDto>(jsonReplies);
            return Ok(reply);
        }
        
        ReplyDto? replyDto = await _commentService.GetReplies(commentId, pageSize, page);
        jsonReplies = JsonSerializer.Serialize(replyDto);

        await _cache.SetStringAsync(key, jsonReplies, cancellationToken);
       
        if (replyDto is null)
        {
            return NotFound(new ApiErrorResponse()
            {
                StatusCode = 404,
                Title = "comment not found.",
                Detail = $"The comment with ID: {commentId} cannot be found"
            });
        }

        return Ok(replyDto);
    }
}