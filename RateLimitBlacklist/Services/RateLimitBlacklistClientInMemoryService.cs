using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using RateLimitBlacklist.AspNetCore.Attributes;

namespace RateLimitBlacklist.AspNetCore.Services
{
    public class RateLimitBlacklistClientInMemoryService : IRateLimitBlacklistClientService
    {

        private readonly IMemoryCache _cache;

        public RateLimitBlacklistClientInMemoryService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public string GenerateClientKey(HttpContext context) => $"{context.Request.Path}_{context.Connection.RemoteIpAddress}";

        public RateLimitBlacklistClient GetOrCreateClientByKey(string key, RateLimitWithBlacklist metadata, HttpContext context)
        {
            RateLimitBlacklistClient client;
            if (!_cache.TryGetValue(key, out client))
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(metadata.TimeWindow)
                };

                client = new RateLimitBlacklistClient(context.Connection.RemoteIpAddress.ToString(), DateTime.UtcNow);

                _cache.Set(key, client, cacheEntryOptions);
            }
            return client;
        }

        public void UpdateClient(string key, RateLimitBlacklistClient client)
        {
            _cache.Set(key, client);
        }
    }
}
