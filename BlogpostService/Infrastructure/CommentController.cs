using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using BlogpostService.Application;
using BlogpostService.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BlogpostService.Infrastructure;

[ApiController]
[Route("/api/comment")]
public class CommentController : ControllerBase
{

    private readonly ICommentService _commentService;
    
    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }
    
    [HttpGet("{commentId:guid}")]
    public async Task<ActionResult<CommentDto>> AddReplyForComment([FromRoute] string commentId,
        [FromQuery] [Required(ErrorMessage = "The includeReplies field must be supplied")] bool? includeReplies,
        [FromQuery] [Required(ErrorMessage = "The depth field must be supplied")] int? depth,
        [FromQuery] [Required(ErrorMessage = "The blog post Id must be supplied")] string? blogpostId)
    {
        
    }
}