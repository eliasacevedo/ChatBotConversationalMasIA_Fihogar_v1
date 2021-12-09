using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Services.DoRequest
{
    public interface IDoRequest{
        Task<HttpResponseMessage> Get(string path, IDictionary<string, string> headers);
        Task<HttpResponseMessage> Post(string path, IDictionary<string, string> headers, string body, string type = "application/json");
        Task<HttpResponseMessage> Post(string path, IDictionary<string, string> headers, IDictionary<string, string> form);
    }
}