using System;

namespace WebQ
{
    public class WebQOptions
    {
        public HttpOptions HttpOptions { get; set; }

        public void UseHttp(Action<HttpOptions> options)
        {
            HttpOptions = new HttpOptions();
            options?.Invoke(HttpOptions);
        }
    }
}
