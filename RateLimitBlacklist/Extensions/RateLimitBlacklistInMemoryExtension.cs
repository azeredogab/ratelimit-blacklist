using Microsoft.Extensions.DependencyInjection;
using RateLimitBlacklist.AspNetCore.Services;

namespace RateLimitBlacklist.Extensions
{
    public static class RateLimitBlacklistInMemoryExtension
    {
        public static IServiceCollection AddRateLimitBlacklistInMemory(this IServiceCollection services)
        {
            return services.AddTransient<IRateLimitBlacklistClientService, RateLimitBlacklistClientInMemoryService>();
        }
    }
}
