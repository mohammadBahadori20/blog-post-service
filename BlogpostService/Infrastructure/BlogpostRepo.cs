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

    public async Task<Author?> GetAuthorByUsername(string username)
    {
        Author? author = await _context.Authors.FirstOrDefaultAsync(a => a.Username == username);
        return author;
    }

    public async Task<Blogpost?> GetBlogpostById(string blogpostId)
    {
        throw new NotImplementedException();
    }

    public async Task AddNewAuthor(Author author)
    {
        await _context.Authors.AddAsync(author);
    }

    public async Task AddNewBlogpostForAuthor(Blogpost blogpost, string authorUsername)
    {
        await _context.Blogposts.AddAsync(blogpost);
    }
}