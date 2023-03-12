using System;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Shared;
using Infrastructure.Network.Contracts;
using Microsoft.Extensions.Options;
using Sender.Services.Contracts;

namespace Sender.Services;

public class TransferService : ITransferService
{
    private readonly IHttpClientWrapper _httpClient;

    private readonly SharedSettings _settings;

    public TransferService(IHttpClientWrapper httpClient, IOptions<SharedSettings> settings)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

        _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
    }

    public async Task<ResultViewModel> PostToRecipientAsync(TransferModel model)
    {
        _httpClient.BaseUrl = _settings.RecipientApi.BaseUrl;
        _httpClient.AuthorizationHeader = _settings.Security.Header;
        _httpClient.AuthorizationValue = $"{model.UserName}:{model.Password}";

        var response = await _httpClient.PostAsync<ResultViewModel>(model.JsonData);

        return response;
    }
}