namespace BlogpostService.fault_tolerance_policies;

public interface IDistributedRedisCache
{
    Task<string?> GetStringAsync(string key);
    Task SetStringAsync(string key, string value);
    Task RemoveAsync(string key);
}