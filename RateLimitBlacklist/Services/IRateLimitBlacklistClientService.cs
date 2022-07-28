using Microsoft.AspNetCore.Http;
using RateLimitBlacklist.AspNetCore.Attributes;

namespace RateLimitBlacklist.AspNetCore.Services
{
    public interface IRateLimitBlacklistClientService
    {
        string GenerateClientKey(HttpContext context);
        RateLimitBlacklistClient GetOrCreateClientByKey(string key, RateLimitWithBlacklist metadata, HttpContext context);
        void UpdateClient(string key, RateLimitBlacklistClient client);
    }
}
