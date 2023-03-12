using System;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Shared;
using Infrastructure.Network.Contracts;
using Microsoft.Extensions.Options;
using Sender.Services.Contracts;

namespace Sender.Services
{
    public class TransferService : ITransferService
    {
        private readonly IHttpClientWrapper _httpClient;

        private readonly  SharedSettings _settings;

        public TransferService(IHttpClientWrapper httpClient, IOptions<SharedSettings> settings)
        {
            this._httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

            this._settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
        }

        public async Task<ResultViewModel> PostToRecipientAsync(TransferModel model)
        {
            this._httpClient.BaseUrl = _settings.RecipientApi.BaseUrl;
            this._httpClient.AuthorizationHeader = _settings.Security.Header;
            this._httpClient.AuthorizationValue = $"{model.UserName}:{model.Password}";

            var response = await _httpClient.PostAsync<ResultViewModel>(model.JsonData);

            return response;
        }
    }
}