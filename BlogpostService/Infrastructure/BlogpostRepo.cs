using BlogpostService.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlogpostService.Infrastructure;

public class BlogpostRepo : IBlogpostRepo
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<BlogpostRepo> _logger;

    public BlogpostRepo(ApplicationDbContext context, ILogger<BlogpostRepo> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Blogpost?> GetBlogpostById(Guid blogpostId)
    {
        try
        {
            return await _context.Blogposts.FindAsync(blogpostId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "In {className}.{methodName} an error occured when accessing the database for blogpost ID:{blogpostId}",
                nameof(BlogpostRepo), nameof(GetBlogpostById), blogpostId);
            throw;
        }
    }

    public async Task AddNewBlogpostForAuthor(Blogpost blogpost)
    {
        await _context.Blogposts.AddAsync(blogpost);
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteBlogpost(Guid blogpostId)
    {
        Blogpost? blogpost = await _context.Blogposts.FirstOrDefaultAsync(b => b.BlogPostId == blogpostId);
        if (blogpost == null)
        {
            return false;
        }

        _context.Blogposts.Remove(blogpost);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Comment?> GetCommentById(Guid commentId)
    {
        return await _context.Comments.FindAsync(commentId);
    }
}