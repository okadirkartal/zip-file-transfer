using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Network.Contracts;
using Core.ZipManagementService;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;
using Sender.Services;
using Sender.Services.Contracts;


namespace CyptoTests
{
    public class TransferserviceTests
    {
        private readonly ITransferService _transferService;

        private readonly IHttpClientWrapper _httpClient;

        private readonly IConfiguration _configuration;

        public TransferserviceTests()
        {
            _httpClient = Substitute.For<IHttpClientWrapper>();

            _configuration = Substitute.For<IConfiguration>();

            _transferService = new TransferService(_httpClient, _configuration);
        }

        [Test]
        public void Constructor_WhenArgumentIsNull_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => new TransferService(null, _configuration));
            Assert.Throws<ArgumentNullException>(() => new TransferService(_httpClient, null));
        }

        [Test]
        public async Task PostToRecipient_WhenIsValid_ReturnsTrue()
        {
            string endpoint = "Post";

            DirectoryModel directoryModel = new DirectoryModel("test", "test");
            TransferModel transferModel = new TransferModel()
            {
                JsonData = JsonConvert.SerializeObject(directoryModel),
                UserName = "demo",
                Password = "test"
            };

            _configuration[Core.Common.Constants.RecipientApiBaseUrl].Returns("http://localhost:5006/api/Recipient/");
            _configuration[Core.Common.Constants.SecurityHeader].Returns("Authorization");
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