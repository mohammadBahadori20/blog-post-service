using BlogpostService.Application;
using BlogpostService.Domain;
using BlogpostService.Infrastructure;


namespace BlogpostService.DependencyInjection;

public static class DependencyInjection
{
    public static void DependencyInjectionMapper(this IServiceCollection service)
    {
        service.AddTransient<IUserServcie,UserService>();
        service.AddTransient<IBlogpostService,Application.BlogpostService>();
        service.AddTransient<IBlogpostRepo,BlogpostRepo>();
        service.AddTransient<ICommentService,CommentService>();
    }
}