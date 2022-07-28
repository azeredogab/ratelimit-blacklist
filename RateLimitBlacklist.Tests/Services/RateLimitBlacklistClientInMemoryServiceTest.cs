using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using RateLimitBlacklist.AspNetCore.Attributes;
using RateLimitBlacklist.AspNetCore.Services;
using System;
using System.Net;
using Xunit;

namespace RateLimitBlacklist.Tests.Services
{
    public class RateLimitBlacklistClientInMemoryServiceTest
    {

        [Fact]
        public void GenerateKey_WithHttpContext_ReturnCorrectKey()
        {
            //Arrange
            var mockMemoryCache = new Mock<IMemoryCache>();
            var mockHttpContext = new DefaultHttpContext();
            mockHttpContext.Request.Path = "/Teste";
            mockHttpContext.Connection.RemoteIpAddress = new IPAddress(new byte[] { 1, 1, 1, 1 });
            var clientService = new RateLimitBlacklistClientInMemoryService(mockMemoryCache.Object);

            //Act
            var clientKey = clientService.GenerateClientKey(mockHttpContext);

            //Assert
            var expectedClientKey = "/Teste_1.1.1.1";
            Assert.Equal(expectedClientKey, clientKey);
        }

        [Fact]
        public void GetOrCreateClientByKey_WithNoClientInTheCache_ReturnANewClient()
        {
            //Arrange
            var mockMemoryCache = new Mock<IMemoryCache>();
            mockMemoryCache.Setup(m => m.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny)).Returns(false);
            mockMemoryCache.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>);

            var mockHttpContext = new DefaultHttpContext();
            mockHttpContext.Request.Path = "/Teste";
            mockHttpContext.Connection.RemoteIpAddress = new IPAddress(new byte[] { 1, 1, 1, 1 });

            var clientService = new RateLimitBlacklistClientInMemoryService(mockMemoryCache.Object);

            var mockMetadata = new RateLimitWithBlacklist(5, 10, 15);
            var key = clientService.GenerateClientKey(mockHttpContext);

            var expectedDatesGratherThen = DateTime.UtcNow;

            //Act
            var client = clientService.GetOrCreateClientByKey(key, mockMetadata, mockHttpContext);

            //Assert
            var expectedIp = "1.1.1.1";
            var expectedIsBlocked = false;
            var expectedNumberOfRequests = 0;

            Assert.Equal(expectedIp, client.IpAddress);
            Assert.Equal(expectedIsBlocked, client.IsBlocked);
            Assert.Equal(expectedNumberOfRequests, client.NumberOfRequestsInTheTimeWindow);
            Assert.True(expectedDatesGratherThen < client.FirstRequestInTheTimeWindow);
        }
    }
}
