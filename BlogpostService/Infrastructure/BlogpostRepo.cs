
using BlogpostService.Domain;

namespace BlogpostService.Infrastructure;

public class BlogpostRepo : IBlogpostRepo
{
    private readonly ApplicationDbContext _context;

    public BlogpostRepo(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Blogpost?> GetBlogpostById(string blogpostId)
    {
        throw new NotImplementedException();
    }

    public async Task AddNewBlogpostForAuthor(Blogpost blogpost)
    {
        await _context.Blogposts.AddAsync(blogpost);
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}