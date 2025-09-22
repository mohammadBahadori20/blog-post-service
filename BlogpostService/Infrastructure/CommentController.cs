using BlogpostService.Application;
using BlogpostService.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BlogpostService.Infrastructure;

[ApiController]
[Route("/api/[controller]")]
public class CommentController : ControllerBase
{

    private readonly ICommentService _commentService;
    
    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpGet("/{commentId}/replies")]
    public async Task<ActionResult<ReplyDto>> GetReplies(Guid commentId, int pageSize = 5, int page = 1)
    {
        ReplyDto? replyDto = await _commentService.GetReplies(commentId, pageSize, page);
        if (replyDto is null)
        {
            return NotFound(new ApiErrorResponse()
            {
                StatusCode = 404,
                Title = "comment not found.",
                Detail = $"The comment with ID: {commentId} cannot be found"
            });
        }

        return replyDto;
    }
}