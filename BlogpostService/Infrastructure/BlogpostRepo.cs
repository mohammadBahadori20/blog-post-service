using BlogpostService.Application.DTOs;
using BlogpostService.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlogpostService.Infrastructure;

public class BlogpostRepo : IBlogpostRepo
{
    private ApplicationDbContext _context;

    public BlogpostRepo(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Blogpost?> GetBlogpostById(string blogpostId)
    {
        throw new NotImplementedException();
    }

    public async Task AddNewBlogpostForAuthor(Blogpost blogpost, string authorUsername)
    {
        await _context.Blogposts.AddAsync(blogpost);
    }
}