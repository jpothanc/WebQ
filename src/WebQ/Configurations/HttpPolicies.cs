namespace WebQ
{
    public class HttpPolicies
    {
       public int Timeout { get; set; }
       public int RetryCount { get; set; }
       public int RetryInterval { get; set; }
       public int CircuitBreakerCount { get; set; }
        public HttpPolicies()
        {
            Timeout = 60;
            RetryCount = 30;
            RetryInterval = 10;
            CircuitBreakerCount = 60;
        }
    }
}
