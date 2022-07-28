namespace RateLimitBlacklist.AspNetCore.Services
{
    public class RateLimitBlacklistClient
    {
        public string IpAddress { get; private set; }
        public DateTime FirstRequestInTheTimeWindow { get; private set; }
        public int NumberOfRequestsInTheTimeWindow { get; private set; }
        public bool IsBlocked { get; private set; }
        public DateTime BlockExpires { get; private set; }

        public RateLimitBlacklistClient(string ipAddress, DateTime firstRequestInTheTimeWindow)
        {
            IpAddress = ipAddress;
            FirstRequestInTheTimeWindow = firstRequestInTheTimeWindow;
            NumberOfRequestsInTheTimeWindow = 0; 
        }

        public void Block(DateTime blockExpires)
        {
            IsBlocked = true;
            BlockExpires = blockExpires;
        }
    
        public void Unblock()
        {
            IsBlocked = false; 
        }

        public void AddRequestCount() => NumberOfRequestsInTheTimeWindow++;

        public void ClearRequests()
        {
            NumberOfRequestsInTheTimeWindow = 0;
            FirstRequestInTheTimeWindow = DateTime.UtcNow;
        }
    }
}
