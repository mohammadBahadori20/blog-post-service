using BlogpostService.Application.DTOs;
using BlogpostService.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace BlogpostService.Application;

public class AuthorService : IAuthorService
{
    private readonly IBlogpostRepo _repo;
    private readonly IConfiguration _configuration;
    private readonly string _host;
    private readonly string _port;
    
    
    public AuthorService(IBlogpostRepo repo,IConfiguration configuration)
    {
        _repo = repo;
        _configuration = configuration;
        _host = _configuration["host"]!;
        _port = _configuration["port"]!;
    }
    
    public async Task<Author?> GetAuthorByUsername(string username)
    {
        var author = await _repo.GetAuthorByUsername(username);
        return author;
    }
    
    public async Task<List<BlogpostDto>?> GetAuthorBlogpostsByUsername(string username)
    {
        Author? author =  await _repo.GetAuthorByUsername(username);

        if (author is null)
        {
            return null;
        }
       
        
        List<BlogpostDto> blogpostDtos  = author.Blogposts.Select(blogpost => new BlogpostDto()
        {
            BlogPostId = blogpost.BlogPostId,
            PublishedAt = blogpost.PublishedAt,
            Content = blogpost.Content,
            Title = blogpost.Title,
            Comments = new Uri($"http://{_host}:{_port}/api/blogpost/{blogpost.BlogPostId}/comments")

        }).ToList();

        return blogpostDtos;
    }

    public async Task AddNewAuthor(AuthorDto authorDto)
    {
        Author author = new Author()
        {
            AuthorId = HttpContext.,
            Username = authorDto.Username
        };

        authorDto.AuthorId = author.AuthorId;
        authorDto.AuthorBlogposts = new Uri($"http://{_host}:{_port}/api/blogpost/{author.Username}");
        
        await _repo.AddNewAuthor(author);
    } 
    
}