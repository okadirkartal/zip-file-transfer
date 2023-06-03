using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Infrastructure.Network.Contracts;

namespace Infrastructure.Network;

public class HttpClientWrapper : IHttpClientWrapper
{
    private readonly IHttpClientFactory _httpClientFactory;

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

        var content = JsonContent.Create(dto);

        using var client = _httpClientFactory.CreateClient();
        InitializeCredentials(client);
        var responseMessage = await client.PostAsync(client.BaseAddress, content);

        var data = await responseMessage.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<TResponseDto>(data);
    }

    private HttpClient InitializeCredentials(HttpClient client)
    {
        client.BaseAddress = new Uri(BaseUrl);

        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(AuthorizationHeader, AuthorizationValue);

        return client;
    }
}