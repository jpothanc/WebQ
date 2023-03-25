namespace WebQ
{
    public class HttpOptions
    {
        public string  BaseUrl { get; set; }
        public string Api { get; set; }
        public DiskOptions DiskOptions { get; set; }
        public HttpPolicies HttpPolicies { get; set; }

        public bool SupportCaching() => DiskOptions != null;
    }
}
