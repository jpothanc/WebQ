using System;
using System.IO;
using System.Threading.Tasks;

namespace WebQ
{
    public class HttpCachedRequest : IHttpRequest
    {
        private IHttpRequest _httpRequest = null;
        private IDiskCache _diskCache = null;
        public HttpCachedRequest(WebQOptions options)
        {
            _httpRequest = new HttpRequest(options);
            _diskCache = new DiskCache(options.HttpOptions.DiskOptions);
        }

        public async Task<string> GetAsync(WebQRequest request)
        {
            string data = "";
            try
            {
                 data = await _httpRequest.GetAsync(request);
                _diskCache.Save(data, request.NameTag);
                return data;
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                data = _diskCache.Read(request.NameTag);
            }

            return data;
        }
    }
}
