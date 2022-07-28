using Microsoft.AspNetCore.Http;
using RateLimitBlacklist.AspNetCore.Middlewares;
using RateLimitBlacklist.AspNetCore.Services;
using System.Net;
using Xunit;
using Moq;

namespace RateLimitBlacklist.Tests
{
    public class RateLimitBlacklistMiddlewareTest
    {
        [Fact]
        public async void InvokeAsync_WithNoMetadata_ShouldCallNextWithArgumentNull()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = "/TesteUrl";
            httpContext.Connection.RemoteIpAddress = new IPAddress(new byte[] { 1, 1, 1, 1 });

            var mockRequestDelegate = new Mock<RequestDelegate>();
            mockRequestDelegate.Setup(d => d(httpContext)).Verifiable();
            var mockClientService = new Mock<IRateLimitBlacklistClientService>();

            var rateLimitBlacklistMiddleware = new RateLimitBlacklistMiddleware(mockRequestDelegate.Object, mockClientService.Object);

            await rateLimitBlacklistMiddleware.InvokeAsync(httpContext);
            mockRequestDelegate.Verify(d => d(httpContext), Times.Once());
        }
    }
}
