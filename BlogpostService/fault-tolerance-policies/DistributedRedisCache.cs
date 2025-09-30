using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Caching.Distributed;
using Polly;
using Polly.Retry;
using Polly.Timeout;

namespace BlogpostService.fault_tolerance_policies;

public class DistributedRedisCache : IDistributedRedisCache
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<DistributedRedisCache> _logger;

    public DistributedRedisCache(IDistributedCache cache, ILogger<DistributedRedisCache> logger)
    {
        _cache = cache;
        _logger = logger;
    }


    public async Task<string?> GetStringAsync(string key, CancellationToken cancellationToken = default)
    {
        var pipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions()
            {
                MaxRetryAttempts = 5,
                BackoffType = DelayBackoffType.Exponential,
                Delay = TimeSpan.FromMicroseconds(200),
                OnRetry = (args) =>
                {
                    _logger.LogWarning(
                        "In {className}.{methodName}, the retry policy has been triggered for:{attemptNumber} times",
                        nameof(DistributedRedisCache), nameof(GetStringAsync), args.AttemptNumber);
                    return default;
                }
            })
            .Build();

        string? value = null;

        await pipeline.ExecuteAsync(async (ct) => { value = await _cache.GetStringAsync(key, token: ct); },
            cancellationToken);

        return value;
    }

    
    
    public async Task SetStringAsync(string key, string value, CancellationToken cancellationToken = default)
    {
        var pipline = new ResiliencePipelineBuilder()
            .AddTimeout(TimeSpan.FromSeconds(1))
            .Build();

          
        var options = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(30))
            .SetSlidingExpiration(TimeSpan.FromMinutes(5));
        
        try
        {
            await pipline.ExecuteAsync(async (innerToken) => await _cache.SetStringAsync(key, value,options ,innerToken),
                cancellationToken);
        }
        catch (TimeoutRejectedException exception)
        {
            _logger.LogWarning(exception,
                "In {className}.{methodName}, the timeout for accessing the redis cache has been triggered",
                nameof(DistributedRedisCache), nameof(SetStringAsync));
        }
        catch (Exception exception)
        {
            _logger.LogError(exception,
                "In {className}.{methodName}, an error occured when accessing the redis caching system",
                nameof(DistributedRedisCache), nameof(SetStringAsync));
            throw;
        }
    }
}