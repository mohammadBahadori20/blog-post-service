using BlogpostService.Application.DTOs;
using BlogpostService.Domain;
using BlogpostService.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace BlogpostService.Application;

public class BlogpostService : IBlogpostService
{
    private readonly IBlogpostRepo _repo;
    private readonly IConfiguration _configuration;
    private readonly IAuthorServcie _authorService;
    private readonly string _host;
    private readonly string _scheme;
    private readonly string _port;

    public BlogpostService(IBlogpostRepo repo, IConfiguration configuration, IAuthorServcie authorService)
    {
        _repo = repo;
        _configuration = configuration;
        _host = _configuration["host"]!;
        _port = _configuration["port"]!;
        _scheme = _configuration["scheme"]!;
        _authorService = authorService;
    }

    public async Task<BlogpostDto?> GetBlogpost(Guid blogpostId)
    {
        var blogpost = await _repo.GetBlogpostById(blogpostId);
        
        if (blogpost is null)
        {
            return null;
        }
        
        return new BlogpostDto()
        {
            BlogPostId = blogpostId,
            Description = blogpost.Description,
            Title = blogpost.Title,
            Comments = new Uri($"{_scheme}://{_host}:{_port}/api/blogpost/{blogpost.BlogPostId}/comments")
        };
    }

    public async Task<List<CommentDto>?> GetBlogpostCommentsById(Guid blogpostId, int pageSize, int page)
    {
        
        Blogpost? blogpost = await _repo.GetBlogpostById(blogpostId);
        
        if (blogpost is null)
        {
            return null;
        }

        List<CommentDto> commentDtos = [];

        if (blogpost.Comments.Count + 1 < pageSize * page)
        {
            return commentDtos;
        }
        
        for (int i = pageSize * (page - 1); i < pageSize * page; ++i)
        {
            commentDtos.Add(new CommentDto()
            {
                AuthorId = blogpost.Comments[i].AuthorId,
                CommentId = blogpost.Comments[i].CommentId,
                Content = blogpost.Comments[i].Content,
                CreatedAt = blogpost.Comments[i].CreatedAt,
                RepliesCount = blogpost.Comments[i].Replies.Count
            });
        }

        return commentDtos;
    }

    public async Task<BlogpostDto> PublishBlogpost(BlogpostDto blogpostDto, Guid authorId)
    {
        Blogpost blogpost = new Blogpost()
        {
            AuthorId = authorId,
            BlogPostId = Guid.NewGuid(),
            Description = blogpostDto.Description,
            Title = blogpostDto.Title,
            PublishedAt = DateTime.UtcNow
        };

        blogpostDto.BlogPostId = blogpost.BlogPostId;
        blogpostDto.Comments = new Uri($"{_scheme}://{_host}:{_port}/api/blogpost/{blogpost.BlogPostId}/comments");
        await _repo.AddNewBlogpostForAuthor(blogpost);
        return blogpostDto;
    }

    public async Task<CommentDto?> AddCommentForBlogpost(CommentDto commentDto, Guid blogpostId, Guid authorId)
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
            CommentId = Guid.NewGuid(),
            Content = commentDto.Content ?? string.Empty,
            CreatedAt = DateTime.UtcNow.Date
        };
        blogpost.AddNewComment(comment);
        await _repo.SaveChanges();
        return commentDto;
    }

    public async Task<bool> DeleteBlogpost(Guid blogpostId)
    {
        return await _repo.DeleteBlogpost(blogpostId);
    }

    public async Task<BlogpostDto?> UpdateBlogpost(UpdatedBlogpostDto updatedBlogpost, Guid blogpostId)
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