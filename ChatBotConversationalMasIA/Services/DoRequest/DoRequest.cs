using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Services.DoRequest
{
    public class DoRequest: IDoRequest{

        private readonly IHttpClientFactory _clientFactory;
        public DoRequest(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<HttpResponseMessage> Get(string path, IDictionary<string, string> headers) {
            var request = new HttpRequestMessage(HttpMethod.Get, path);
            
            foreach (var header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            return response;
        }

        public async Task<HttpResponseMessage> Post(string path, IDictionary<string, string> headers, string body, string type = "application/json") {
            var content = new StringContent(body, Encoding.UTF8, type);

            foreach (var header in headers)
            {
                content.Headers.Add(header.Key, header.Value);
            }

            var client = _clientFactory.CreateClient();
            var response = await client.PostAsync(path, content);
            return response;
        }

        public async Task<HttpResponseMessage> Post(string path, IDictionary<string, string> headers, IDictionary<string, string> form)
        {
            var req = new HttpRequestMessage(HttpMethod.Post, path) { Content = new FormUrlEncodedContent(form) };
            foreach (var header in headers)
            {
                req.Headers.Add(header.Key, header.Value);
            }

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(req);
            return response;
        }
        
    }
}