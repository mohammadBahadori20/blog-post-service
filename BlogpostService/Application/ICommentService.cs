using BlogpostService.Application.DTOs;
using BlogpostService.Domain;

namespace BlogpostService.Application;

public interface ICommentService
{
    public Task<ReplyDto?> GetReplies(Guid commentId, int pageSize, int page);
}