using BlogpostService.Application;
using BlogpostService.BlogpostService.Protos;
using Grpc.Core;
using Grpc.Net.Client;

namespace BlogpostService.Infrastructure;

public class AuthorService :IAuthorServcie
{

    private readonly UserService.UserServiceClient _client;
    private readonly ILogger<AuthorService> _logger;

    public AuthorService(UserService.UserServiceClient client, ILogger<AuthorService> logger)
    {
        _client = client;
        _logger = logger;
    }
    
    public async Task<AuthorDetailsResponse> GetUserById(AuthorDetailsRequest request)
    {
        try
        {
            var response = await _client.GetUserDetailsAsync(request);
            return new AuthorDetailsResponse()
            {
                UserId = response.UserId,
                FirstName = response.FirstName,
                LastName = response.LastName,
                PhoneNumber = response.PhoneNumber
            };
        }
        catch (RpcException ex)
        {
            _logger.LogError(ex, "Error calling gRPC service for user {RequestUserId}", request.UserId);
            throw;
        }

    }
}