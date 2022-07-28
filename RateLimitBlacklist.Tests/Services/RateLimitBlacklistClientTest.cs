using RateLimitBlacklist.AspNetCore.Services;
using System;
using Xunit;

namespace RateLimitBlacklist.Tests.Services
{
    public class RateLimitBlacklistClientTest
    {

        [Fact]
        public void Instance_WithInitialValues_ReturnCorrectValues()
        {
            var ip = "1.1.1.1";
            var firstRequest = DateTime.UtcNow; 
            var client = new RateLimitBlacklistClient(ip, firstRequest);

            Assert.False(client.IsBlocked);
            Assert.Equal(firstRequest, client.FirstRequestInTheTimeWindow);
            Assert.False(client.IsBlocked);
            Assert.Equal(0, client.NumberOfRequestsInTheTimeWindow);
            Assert.True(DateTime.UtcNow > client.BlockExpires);
        }

        [Fact]
        public void Block_WithDateTime_ReturnClientBlocked()
        {
            var client = new RateLimitBlacklistClient("1.1.1.1", DateTime.UtcNow);
            var expirationDate = DateTime.UtcNow.AddMinutes(10); 
            client.Block(expirationDate);

            Assert.True(client.IsBlocked);
            Assert.Equal(expirationDate, client.BlockExpires);
        }


        [Fact]
        public void Unblock_CalledOnce_ReturnClientUnblocked()
        {
            var client = new RateLimitBlacklistClient("1.1.1.1", DateTime.UtcNow);
            var expirationDate = DateTime.UtcNow.AddMinutes(10);
            client.Block(expirationDate);
            client.Unblock();
            Assert.False(client.IsBlocked);
        }


        [Fact]
        public void AddRequestCount_Called3Times_ReturnNumberOfRequestsInTheTimeWindowEqual3()
        {
            var client = new RateLimitBlacklistClient("1.1.1.1", DateTime.UtcNow);
            client.AddRequestCount();
            client.AddRequestCount();
            client.AddRequestCount();

            Assert.Equal(3, client.NumberOfRequestsInTheTimeWindow);
        }


        [Fact]
        public void ClearRequests_CalledOnce_ClearClientObject()
        {
            var firstRequestDateTime = DateTime.UtcNow; 
            var client = new RateLimitBlacklistClient("1.1.1.1", firstRequestDateTime);
            client.AddRequestCount();
            client.AddRequestCount();
            client.AddRequestCount();

            client.ClearRequests(); 

            Assert.Equal(0, client.NumberOfRequestsInTheTimeWindow);
            Assert.True(firstRequestDateTime < client.FirstRequestInTheTimeWindow);
        }
    }
}
