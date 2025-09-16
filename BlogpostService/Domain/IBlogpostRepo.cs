namespace BlogpostService.Domain;

public interface IBlogpostRepo
{
    Task<Blogpost?> GetBlogpostById(string blogpostId);

    public Task AddNewBlogpostForAuthor(Blogpost blogpost);

    public Task SaveChanges();

    public Task<bool>  DeleteBlogpost(string blogpostId);
}