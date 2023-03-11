using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Shared;
using Infrastructure.Network.Contracts;
using Infrastructure.Services;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using Sender.Services;
using Sender.Services.Contracts; 


namespace tests.ZipFileTests
{

    public class TransferServiceTests
    {
        private readonly ITransferService _transferService;

        private readonly IHttpClientWrapper _httpClient;

        private readonly IOptions<SharedSettings> _settings;

        public TransferServiceTests()
        {
            _httpClient = Substitute.For<IHttpClientWrapper>();

            _settings = Options.Create(new SharedSettings
            {
                   RecipientApi = new RecipientApi() {  BaseUrl = "http://localhost:5006/api/Recipient/"},
                   Security = new Security(){ CryptoKey ="8137081371813720"}
            });

            _transferService = new TransferService(_httpClient, _settings);
        }

        [Test]
        public void Constructor_WhenArgumentIsNull_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => new TransferService(null, _settings));
            Assert.Throws<ArgumentNullException>(() => new TransferService(_httpClient, null));
        }

        [Test]
        public async Task PostToRecipient_WhenIsValid_ReturnsTrue()
        {
            string endpoint = "Post";

            DirectoryModel directoryModel = new DirectoryModel("test", "test");
            TransferModel transferModel = new TransferModel()
            {
                JsonData = JsonSerializer.Serialize(directoryModel),
                UserName = "demo",
                Password = "test"
            };

            SharedSettings settings = _settings.Value;
            settings.RecipientApi.BaseUrl.Returns("http://localhost:5077/api/Recipient/");
            settings.Security.Header.Returns("Authorization");
            _httpClient.PostAsync<ResultViewModel>(endpoint, transferModel.JsonData).Returns(new ResultViewModel());


            var result = await _transferService.PostToRecipientAsync(endpoint, transferModel);

            Assert.LessOrEqual(0, result.Errors.Count());
        }


        [Test]
        public void PostToRecipientAsync_WhenArgumentIsNull_ThrowsNullReferenceException()
        {
            // act & assert
            Assert.ThrowsAsync<NullReferenceException>(() => _transferService.PostToRecipientAsync(null, null));
        }
    }
}