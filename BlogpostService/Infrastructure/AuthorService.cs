using System.Text;
using BlogpostService.Application.DTOs;
using BlogpostService.BlogpostService.Protos;
using Grpc.Core;

namespace BlogpostService.Infrastructure;

public class AuthorService :IUserServcie
{
    public UserDto GetUserById(string userId)
    {
        throw new NotImplementedException();
    }
}