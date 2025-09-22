namespace BlogpostService.Domain;

public interface IBlogpostRepo
{
    Task<Blogpost?> GetBlogpostById(Guid blogpostId);

    public Task AddNewBlogpostForAuthor(Blogpost blogpost);

    public Task SaveChanges();

    public Task<bool>  DeleteBlogpost(Guid blogpostId);

    public Task<Comment?> GetCommentById(Guid commentId);
}