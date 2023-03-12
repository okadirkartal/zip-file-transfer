
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Infrastructure.Network.Contracts;

namespace Infrastructure.Network;

public class HttpClientWrapper : IHttpClientWrapper
{
    private readonly IHttpClientFactory _httpClientFactory = null!; 
    public HttpClientWrapper(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public string BaseUrl { get; set; }

    public string AuthorizationHeader { get; set; }

    public string AuthorizationValue { get; set; }


    public async Task<TResponseDto?> PostAsync<TResponseDto>(object dto)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));

        JsonContent content = JsonContent.Create(dto); 

        using HttpClient _client  = _httpClientFactory.CreateClient();
        InitializeCredentials(_client);
        HttpResponseMessage responseMessage = await _client.PostAsync(_client.BaseAddress, content);

        var data = await responseMessage.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<TResponseDto>(data);
    }

    private HttpClient InitializeCredentials(HttpClient _client)
    { 
        _client.BaseAddress = new Uri(BaseUrl);

        _client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationHeader, AuthorizationValue);

        return _client;
    }
}