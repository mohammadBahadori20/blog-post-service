using BlogpostService.Application.DTOs;
using BlogpostService.Domain;
using Microsoft.AspNetCore.Mvc;

namespace BlogpostService.Infrastructure;

public interface IBlogpostService
{
    public Task<List<CommentDto>?> GetBlogpostCommentsById(Guid blogpostId, int pageSize, int page);

    public Task<BlogpostDto> PublishBlogpost(BlogpostDto blogpostDto, Guid blogpostId);

    public Task<CommentDto?> AddCommentForBlogpost(CommentDto commentDto, Guid blogpostId, Guid authorId);

    public Task<bool> DeleteBlogpost(Guid blogpostId);

    public Task<BlogpostDto?> UpdateBlogpost(UpdatedBlogpostDto updatedBlogpost, Guid blogpostId);
    
}