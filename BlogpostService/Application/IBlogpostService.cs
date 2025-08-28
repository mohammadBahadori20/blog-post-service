using BlogpostService.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BlogpostService.Infrastructure;

public interface IBlogpostService
{
    public Task<List<CommentDto>?> GetBlogpostComments(string blogpostId);

    public Task<BlogpostDto> AddNewBlogpostForAuthor(BlogpostDto blogpostDto, string authorUsername);

}