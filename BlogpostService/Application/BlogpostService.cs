using BlogpostService.Application.DTOs;
using BlogpostService.Domain;
using BlogpostService.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace BlogpostService.Application;

public class BlogpostService : IBlogpostService
{
    private readonly IBlogpostRepo _repo;
    private readonly IConfiguration _configuration;
    private readonly string _host;
    private readonly string _port;

    public BlogpostService(IBlogpostRepo repo, IConfiguration configuration)
    {
        _repo = repo;
        _configuration = configuration;
        _host = configuration["host"]!;
        _port = configuration["port"]!;
    }

    public async Task<List<CommentDto>?> GetBlogpostComments(string blogpostId)
    {
        Blogpost? blogpost = await _repo.GetBlogpostById(blogpostId);
        if (blogpost is null)
        {
            return null;
        }

        return blogpost.Comments.Select(cm => new CommentDto()
        {
            Content = cm.Content,
            Author = cm.Author,
            CreatedAt = cm.CreatedAt,
            Replies = cm.Replies
        }).ToList();
    }

    public async Task<BlogpostDto> AddNewBlogpostForAuthor(BlogpostDto blogpostDto, string authorUsername)
    {
        Blogpost blogpost = new Blogpost()
        {
            BlogPostId = Guid.NewGuid(),
            Content = blogpostDto.Content,
            Title = blogpostDto.Title,
            PublishedAt = blogpostDto.PublishedAt
        };

        blogpostDto.BlogPostId = blogpost.BlogPostId;
        blogpostDto.Comments = new Uri($"http://{_host}:{_port}/api/blogpost/{blogpost.BlogPostId}");
        await _repo.AddNewBlogpostForAuthor(blogpost, authorUsername);
        return blogpostDto;
    }
}