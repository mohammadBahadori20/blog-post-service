using BlogpostService.Application.DTOs;

namespace BlogpostService.Infrastructure;

public interface IUserServcie
{
    public UserDto GetUserById(String userId);
}