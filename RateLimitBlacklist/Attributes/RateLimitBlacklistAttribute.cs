namespace RateLimitBlacklist.AspNetCore.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RateLimitWithBlacklist : Attribute
    {
        public int MaxRequests { get; set; }
        public int TimeWindow { get; set; }
        public int BlockTime { get; set; }

        public RateLimitWithBlacklist(int maxRequests, int timeWindow, int blockTime)
        {
            MaxRequests = maxRequests;
            TimeWindow = timeWindow;
            BlockTime = blockTime;
        }
    }
}
