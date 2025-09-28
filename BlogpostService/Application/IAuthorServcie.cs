using BlogpostService.Application.DTOs;
using BlogpostService.BlogpostService.Protos;

namespace BlogpostService.Application;

public interface IAuthorServcie
{
    public Task<AuthorDetailsResponse> GetUserById(AuthorDetailsRequest request);
}