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
    private readonly string _scheme;
    private readonly string _port;

    public BlogpostService(IBlogpostRepo repo, IConfiguration configuration, IUserServcie userService)
    {
        _repo = repo;
        _configuration = configuration;
        _host = _configuration["host"]!;
        _port = _configuration["port"]!;
        _scheme = _configuration["scheme"]!;
        _userService = userService;
    }

    public async Task<List<CommentDto>?> GetBlogpostCommentsById(string blogpostId)
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

    public async Task<BlogpostDto> PublishBlogpost(BlogpostDto blogpostDto, string authorId)
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
        blogpostDto.Comments = new Uri($"{_scheme}://{_host}:{_port}/api/blogpost/{blogpost.BlogPostId}/comments");
        await _repo.AddNewBlogpostForAuthor(blogpost);
        return blogpostDto;
    }

    public async Task<CommentDto?> AddCommentForBlogpost(CommentDto commentDto, string blogpostId, string authorId)
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

    public async Task<bool> DeleteBlogpost(string blogpostId)
    {
        return await _repo.DeleteBlogpost(blogpostId);
    }

    public async Task<BlogpostDto?> UpdateBlogpost(UpdatedBlogpostDto updatedBlogpost,string blogpostId)
    {
        Blogpost? blogpost = await _repo.GetBlogpostById(blogpostId);
        if (blogpost == null)
        {
            return null;
        }

        blogpost.Description = updatedBlogpost.Description;
        blogpost.Title = updatedBlogpost.Title;

        await _repo.SaveChanges();


        return new BlogpostDto()
        {
            BlogPostId = blogpostId,
            Title = blogpost.Title,
            Description = blogpost.Description,
            Comments = new Uri($"{_scheme}://{_host}:{_port}/api/blogpost/{blogpost.BlogPostId}/comments")
        };
    }
    
}