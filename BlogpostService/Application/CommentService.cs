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
    
    
    public async Task<ReplyDto?> GetReplies(Guid commentId,int pageSize ,int page)
    {
        Comment? comment = await _repo.GetCommentById(commentId);
        if (comment is null)
        {
            return null;
        }

        List<CommentDto> commentDtos = [];
        for (int i = pageSize * (page - 1); i < pageSize * page; ++i)
        {
            
           commentDtos.Add(new CommentDto()
           {
               AuthorId = comment.Replies[i].AuthorId,
               CommentId = comment.Replies[i].CommentId,
               Content = comment.Replies[i].Content,
               CreatedAt = comment.Replies[i].CreatedAt,
               RepliesCount = comment.Replies[i].Replies.Count
           });
        }

        return new ReplyDto()
        {
            Replies = commentDtos,
            RepliesCount = commentDtos.Count,
            HasMore = pageSize * page < comment.Replies.Count,
            NextPage = page * pageSize < comment.Replies.Count ? page + 1 : null
        };
    }
}