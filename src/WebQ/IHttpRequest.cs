using System.Threading.Tasks;

namespace WebQ
{
    public interface IHttpRequest
    {
        Task<string> GetAsync(WebQRequest request);
    }
}
