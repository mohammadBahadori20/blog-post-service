namespace BlogpostService.fault_tolerance_policies;

public interface IDistributedRedisCache
{
    Task<string?> GetStringAsync(string key,CancellationToken cancellationToken = default);
    Task SetStringAsync(string key, string value,CancellationToken cancellationToken = default);
}