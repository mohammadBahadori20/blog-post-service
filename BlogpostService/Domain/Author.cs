namespace BlogpostService.Domain;

public class Author
{
    public Guid? AuthorId { get; set; }
    public string? Username { get; set; }
    public List<Blogpost> Blogposts { get; set; }
    public List<Comment> Comments { get; set; }
}