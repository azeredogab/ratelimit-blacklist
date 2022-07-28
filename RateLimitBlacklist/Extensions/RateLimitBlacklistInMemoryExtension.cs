using Microsoft.Extensions.DependencyInjection;
using RateLimitBlacklist.AspNetCore.Services;

namespace RateLimitBlacklist.Extensions
{
    public static class RateLimitBlacklistInMemoryExtension
    {
        public static void AddRateLimitBlacklistInMemory(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddTransient<IRateLimitBlacklistClientService, RateLimitBlacklistClientInMemoryService>();
        }
    }
}
