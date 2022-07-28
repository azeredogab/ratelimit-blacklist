using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using RateLimitBlacklist.AspNetCore.Attributes;
using RateLimitBlacklist.AspNetCore.Services;
using System.Net;

namespace RateLimitBlacklist.AspNetCore.Middlewares
{
    public class RateLimitBlacklistMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IRateLimitBlacklistClientService _client;

        public RateLimitBlacklistMiddleware(RequestDelegate next, IRateLimitBlacklistClientService client)
        {
            _next = next;
            _client = client;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
            var metadata = endpoint?.Metadata.GetMetadata<RateLimitWithBlacklist>();

            if (metadata is null)
            {
                await _next(context);
                return;
            }

            var key = _client.GenerateClientKey(context);
            var client = _client.GetOrCreateClientByKey(key, metadata, context);

            client.AddRequestCount();

            if (client.IsBlocked)
            {
                if (DateTime.UtcNow < client.BlockExpires)
                {
                    context.Response.StatusCode = (int) HttpStatusCode.TooManyRequests;
                    return;
                }

                client.Unblock();
                client.ClearRequests();
            }

            if (client.NumberOfRequestsInTheTimeWindow > metadata.MaxRequests)
            {

                client.Block(DateTime.UtcNow.AddMinutes(metadata.BlockTime));
            }

            _client.UpdateClient(key, client);

            await _next(context);
        }
    }
}
