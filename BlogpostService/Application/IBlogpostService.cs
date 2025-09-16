using BlogpostService.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BlogpostService.Infrastructure;

public interface IBlogpostService
{
    public Task<List<CommentDto>?> GetBlogpostCommentsById(string blogpostId);

    public Task<BlogpostDto> PublishBlogpost(BlogpostDto blogpostDto, string authorUsername);

    public Task<CommentDto?> CreateCommentForBlogpost(CommentDto commentDto, string blogpostId, string authorId);

    public Task<bool> DeleteBlogpost(string blogpostId);
}