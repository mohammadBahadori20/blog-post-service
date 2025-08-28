using BlogpostService.Application;
using BlogpostService.Domain;
using BlogpostService.Infrastructure;


namespace BlogpostService.DependencyInjection;

public static class DependencyInjection
{
    public static void DependencyInjectionMapper(this IServiceCollection service)
    {
        service.AddTransient<IAuthorService,AuthorService>();
        service.AddTransient<IBlogpostService,Application.BlogpostService>();
        service.AddTransient<IBlogpostRepo,BlogpostRepo>();


    }
}