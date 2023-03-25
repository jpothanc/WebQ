## WebQ .NET Core Class Library

WebQ is a lightweight class library that provides support for HTTP queries and cache results on the local disk for resiliency. With WebQ, you can configure how many days of data you want to cache and have confidence in your application’s ability to withstand any downtime from your REST endpoint.

Features
Supports HTTP queries
Caches results on local disk for resiliency
Returns data from local disk cache if REST endpoint is down
Configurable cache duration
Supports standard Polly policies like timeout, retry attempt, and circuit breaker

Getting Started
To get started with WebQ, simply add a reference to the WebQ.dll file in your .NET Core project. Once referenced, you can configure the application to use your REST endpoint and specify how many days of data you want to cache. Once configured, WebQ will handle HTTP queries and store the results on the local disk for later use. If the REST endpoint is unavailable, WebQ will use the cached data to ensure that your application keeps running smoothly.

Usage

```cs
using WebQ;

Console.WriteLine("WebQ Test");

var webQ = WebQ.WebQ.Create(x=>
{

    x.UseHttp(x =>
    {
        x.BaseUrl = "https://jsonplaceholder.typicode.com";
        x.DiskOptions = new DiskOptions()
        {
            CachePath = $"{Path.Combine(Path.GetTempPath(),"WebQ")}",
            BackupDays = 3
        };
        x.HttpPolicies = new HttpPolicies()
        {
            Timeout = 60,
            RetryCount = 10,
            RetryInterval = 10,
            CircuitBreakerCount = 30
        };
    });
   
});

var data = await webQ.GetAsync(new WebQRequest()
{
    Url = "posts?_limit=100",
    NameTag = "posts"

});
Console.WriteLine(data);
Console.ReadLine();
```

Conclusion
WebQ is a great solution for anyone who relies on REST endpoints for their applications. With its cache functionality and support for standard Polly policies like timeout, retry attempt, and circuit breaker, WebQ provides an extra layer of resiliency that can help you avoid downtime and keep your application running smoothly. As a .NET Core class library, WebQ can be easily used in other .NET Core projects, providing a simple and effective solution for handling REST endpoint outages.
