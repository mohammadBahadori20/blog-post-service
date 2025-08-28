using BlogpostService.Application.DTOs;
using BlogpostService.Domain;

namespace BlogpostService.Application;

public interface IAuthorService
{
    public Task<List<BlogpostDto>?> GetAuthorBlogpostsByUsername(string username);
    
    public Task<Author?> GetAuthorByUsername(string username);

    public Task AddNewAuthor(AuthorDto authorDto);
}