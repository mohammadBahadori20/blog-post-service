namespace BlogpostService.Domain;

public interface IBlogpostRepo
{
    Task<Author?> GetAuthorByUsername(string username);

    Task<Blogpost?> GetBlogpostById(string blogpostId);

    public Task AddNewAuthor(Author author);

    public Task AddNewBlogpostForAuthor(Blogpost blogpost, string authorUsername);
}