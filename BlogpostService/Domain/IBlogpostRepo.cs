namespace BlogpostService.Domain;

public interface IBlogpostRepo
{
    Task<Blogpost?> GetBlogpostById(string blogpostId);

    public Task AddNewBlogpostForAuthor(Blogpost blogpost, string authorId);
}