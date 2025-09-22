using BlogpostService.Application.DTOs;
using BlogpostService.Domain;

namespace BlogpostService.Application;

public class CommentService : ICommentService
{
    private readonly IBlogpostRepo _repo;
    
    public CommentService(IBlogpostRepo repo)
    {
        _repo = repo;
    }
    
    
    public async Task<CommentDto?> GetComment(string commentId,string blogpostId ,bool? includeReplies, int? depth)
    {
        Blogpost? blogpost = await _repo.GetBlogpostById(blogpostId);
        if (blogpost == null)
        {
            return null;
        }

        Comment? comment = blogpost.Comments.Find(c => c.CommentId == commentId);
        if (comment == null)
        {
            return null;
        }

        

    }
}