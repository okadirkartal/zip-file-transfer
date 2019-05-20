using System;
using System.Threading.Tasks;
using Core.Entities;
using Core.Network.Contracts;
using Microsoft.Extensions.Configuration;
using Sender.Services.Contracts;

namespace Sender.Services
{
    public class TransferService : ITransferService
    {
        private readonly IHttpClientWrapper _httpClient;

        private readonly IConfiguration _configuration;

        public TransferService(IHttpClientWrapper httpClient, IConfiguration configuration)
        {
            this._httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

            this._configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<ResultViewModel> PostToRecipientAsync(string endPoint, TransferModel model)
        {
            this._httpClient.BaseUrl = this._configuration[Core.Common.Constants.RecipientApiBaseUrl];
            this._httpClient.AuthorizationHeader = this._configuration[Core.Common.Constants.SecurityHeader];
            this._httpClient.AuthorizationValue = $"{model.UserName}:{model.Password}";

            var response = await _httpClient.PostAsync<ResultViewModel>(endPoint, model.JsonData);

            return response;
        }
    }
}