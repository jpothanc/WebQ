using System;
using System.Threading.Tasks;
using WebQ;
namespace WebQ
{
    public class WebQ
    {
        private IHttpRequest _httpRequest;
        public WebQ(WebQOptions options)
        {
            _httpRequest = options.HttpOptions.SupportCaching() ? new HttpCachedRequest(options) : 
                (IHttpRequest)new HttpRequest(options);
        }

        public static WebQ Create(Action<WebQOptions> options)
        {
            WebQOptions webQOptions = new WebQOptions();
            options?.Invoke(webQOptions);
            return new WebQ(webQOptions);
        }

        public async Task<string> GetAsync(WebQRequest request)
        {
            return await _httpRequest?.GetAsync(request);
        }
    }
}
