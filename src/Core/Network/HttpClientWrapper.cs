using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Core.Network.Contracts
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private HttpClient _client;

        public string BaseUrl { get; set; }

        public string AuthorizationHeader { get; set; }

        public string AuthorizationValue { get; set; }

        public HttpClientWrapper()
        {
        }


        public async Task<TResponseDto> PostAsync<TResponseDto>(string endpoint, string dto)
        {
            if (string.IsNullOrEmpty(endpoint)) throw new ArgumentNullException(nameof(endpoint));

            if (dto == null) throw new ArgumentNullException(nameof(dto));


            string content = JsonConvert.SerializeObject(dto);

            HttpContent httpContent = new StringContent(content, Encoding.UTF8, "application/json");

            _client = GetHttpClient();


            HttpResponseMessage responseMessage = await _client.PostAsync(endpoint, httpContent);

            _client.Dispose();

            var data = await responseMessage.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TResponseDto>(data);
        }

        public void AddHeader(string name, string value)
        {
            if (!_client.DefaultRequestHeaders.Contains(name))
                _client.DefaultRequestHeaders.Add(name, value);
        }

        public void SetAuthorization(string authorizationType, string userName, string password)
        {
            this._client.DefaultRequestHeaders.Add(authorizationType,
                "Basic " + $"{userName}:{password}");
        }


        private HttpClient GetHttpClient()
        {
            _client = new HttpClient();

            _client.BaseAddress = new Uri(BaseUrl);

            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(AuthorizationHeader, AuthorizationValue);

            return _client;
        }
    }
}