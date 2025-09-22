using BlogpostService.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BlogpostService.Infrastructure;

public interface IBlogpostService
{
    public Task<List<CommentDto>?> GetBlogpostCommentsById(Guid blogpostId, int pageSize, int page);

    public Task<BlogpostDto> PublishBlogpost(BlogpostDto blogpostDto, string authorUsername);

    public Task<CommentDto?> AddCommentForBlogpost(CommentDto commentDto, string blogpostId, string authorId);

    public Task<bool> DeleteBlogpost(string blogpostId);

    public Task<BlogpostDto?> UpdateBlogpost(UpdatedBlogpostDto updatedBlogpost, string blogpostId);
}