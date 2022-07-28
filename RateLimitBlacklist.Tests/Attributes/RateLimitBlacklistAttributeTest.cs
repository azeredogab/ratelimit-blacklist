using RateLimitBlacklist.AspNetCore.Attributes;
using Xunit;

namespace RateLimitBlacklist.Tests.Attributes
{
    public class RateLimitBlacklistAttributeTest
    {
        [Fact]
        public void Constructor_WithRightParams_PutValuesInTheRightPlaces()
        {
            var underTest = new RateLimitWithBlacklist(5, 10, 15);

            Assert.Equal(10, underTest.MaxRequests);
            Assert.Equal(10, underTest.TimeWindow);
            Assert.Equal(15, underTest.BlockTime);
        }
    }
}
