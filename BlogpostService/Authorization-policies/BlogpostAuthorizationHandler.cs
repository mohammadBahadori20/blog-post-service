using BlogpostService.Domain;
using Microsoft.AspNetCore.Authorization;

namespace BlogpostService.Properties.Authorization_policies;

public class BlogpostAuthorizationHandler : AuthorizationHandler<BlogpostAuthorizationRequirement,Guid>
{

    private readonly IBlogpostRepo _repo;

    public BlogpostAuthorizationHandler(IBlogpostRepo repo)
    {
        _repo = repo;
    }
    
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, BlogpostAuthorizationRequirement requirement,
        Guid blogpostId)
    {
        Blogpost? blogpost = await _repo.GetBlogpostById(blogpostId);
        string authorId = context.User.FindFirst(a => a.Type == "sub")!.Value;
        if (blogpost is null || authorId != blogpost.AuthorId.ToString())
        {
            context.Fail();
        }
        else
        {
            context.Succeed(requirement);
        }

    }
}