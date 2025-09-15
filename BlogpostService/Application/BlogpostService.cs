using BlogpostService.Application.DTOs;
using BlogpostService.Domain;
using BlogpostService.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace BlogpostService.Application;

public class BlogpostService : IBlogpostService
{
    private readonly IBlogpostRepo _repo;
    private readonly IConfiguration _configuration;
    private readonly IUserServcie _userService;
    private readonly string _host;
    private readonly string _port;

    public BlogpostService(IBlogpostRepo repo, IConfiguration configuration,IUserServcie userService)
    {
        _repo = repo;
        _configuration = configuration;
        _host = _configuration["host"]!;
        _port = _configuration["port"]!;
        _userService = userService;
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
            CreatedAt = cm.CreatedAt,
            Replies = cm.Replies
        }).ToList();
    }

    public async Task<BlogpostDto> AddNewBlogpostForAuthor(BlogpostDto blogpostDto, string authorId)
    {
        Blogpost blogpost = new Blogpost()
        {
            AuthorId = authorId,
            BlogPostId = Guid.NewGuid().ToString(),
            Description = blogpostDto.Description,
            Title = blogpostDto.Title,
            PublishedAt = DateTime.UtcNow
        };

        blogpostDto.BlogPostId = blogpost.BlogPostId;
        blogpostDto.Comments = new Uri($"http://{_host}:{_port}/api/blogpost/{blogpost.BlogPostId}");
        await _repo.AddNewBlogpostForAuthor(blogpost);
        return blogpostDto;
    }

    public async Task<CommentDto?> AddNewCommentForBlogpost(CommentDto commentDto, string blogpostId,string authorId)
    {
        Blogpost? blogpost = await _repo.GetBlogpostById(blogpostId);
        if (blogpost == null)
        {
            return null;
        }
        
        Comment comment = new Comment()
        {
            AuthorId = authorId,
            BlogPostId = blogpostId,
            CommentId = Guid.NewGuid().ToString(),
            Content = commentDto.Content ?? string.Empty,
            CreatedAt = DateTime.UtcNow.Date
        };
        blogpost.AddNewComment(comment);
        await _repo.SaveChanges();
        return commentDto;
    }
}