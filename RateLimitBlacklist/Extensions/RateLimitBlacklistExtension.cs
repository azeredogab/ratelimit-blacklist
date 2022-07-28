using Microsoft.AspNetCore.Builder;
using RateLimitBlacklist.AspNetCore.Middlewares;

namespace RateLimitBlacklist.AspNetCore.Extensions
{
    public static class RateLimitBlacklistExtension
    {
        public static IApplicationBuilder UseRateLimitBlacklist(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RateLimitBlacklistMiddleware>();
        }
    }
}
