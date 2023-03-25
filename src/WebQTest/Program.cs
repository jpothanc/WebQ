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
//https://jsonplaceholder.typicode.com/posts?_limit=10
var data = await webQ.GetAsync(new WebQRequest()
{
    Url = "posts?_limit=100",
    NameTag = "example"

});
Console.WriteLine(data);
Console.ReadLine();