using Polly;
using Polly.Timeout;
using Polly.Wrap;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WebQ.Internals;

namespace WebQ
{
    public class HttpRequest : IHttpRequest
    {
        private HttpOptions _options;
        private AsyncPolicyWrap _policyWrap;
        public HttpRequest(WebQOptions options)
        {
            _options = options.HttpOptions;
            var timeoutPolicy = Policy.TimeoutAsync(options.HttpOptions.HttpPolicies.Timeout);

            var retryPolicy = Policy.Handle<HttpRequestException>()
                .Or<TimeoutRejectedException>()
                .WaitAndRetryAsync(options.HttpOptions.HttpPolicies.RetryCount,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, options.HttpOptions.HttpPolicies.RetryInterval)));

            var circuitBreakerPolicy = Policy.Handle<Exception>()
             .CircuitBreakerAsync(options.HttpOptions.HttpPolicies.CircuitBreakerCount, 
             TimeSpan.FromSeconds(options.HttpOptions.HttpPolicies.RetryInterval));

            _policyWrap = Policy.WrapAsync(timeoutPolicy,retryPolicy, circuitBreakerPolicy);

        }
        public async Task<string> GetAsync(WebQRequest request)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var url = Helper.UriCombine(_options.BaseUrl, request.Url);

                    HttpResponseMessage response = await _policyWrap.ExecuteAsync(async () =>
                    {
                        return await client.GetAsync(url);
                    });

                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        Console.WriteLine($"Failed with status code: {response.StatusCode}");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Exception caught: {e.Message}");
                    throw;
                }
            }
            return "";
        }

    }
    
}
