using System;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Network.Contracts;
using Microsoft.Extensions.Options;
using Sender.Services.Contracts;

namespace Sender.Services;

public class TransferService : ITransferService
{
    private readonly IHttpClientWrapper _httpClient;

    private readonly IOptions<ApplicationOptions> _settings;

    public TransferService(IHttpClientWrapper httpClient, IOptions<ApplicationOptions> settings)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _settings = settings ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<ResultViewModel> PostToRecipientAsync(TransferModel model)
    {
        _httpClient.BaseUrl = _settings.Value.RecipientApi.BaseUrl;
        _httpClient.AuthorizationHeader = _settings.Value.Security.Header;
        _httpClient.AuthorizationValue = $"{model.UserName}:{model.Password}";

        var response = await _httpClient.PostAsync<ResultViewModel>(model.JsonData);

        return response;
    }
}