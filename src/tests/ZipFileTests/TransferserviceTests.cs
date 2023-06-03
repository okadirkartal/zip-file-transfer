using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities; 
using Infrastructure.Network.Contracts;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using Sender.Services;
using Sender.Services.Contracts;

namespace tests.ZipFileTests;

public class TransferServiceTests
{
    private readonly IHttpClientWrapper _httpClient;

    private readonly IOptions<ApplicationOptions> _settings;
    
    private readonly ITransferService _transferService;

    public TransferServiceTests()
    {
        _httpClient = Substitute.For<IHttpClientWrapper>();

        _settings = Options.Create(new ApplicationOptions
        {
            RecipientApi = new  RecipientApi {  BaseUrl = "http://localhost:5077/api/Recipient/" },
            Security = new Security { CryptoKey = "8137081371813720", Header = "Authorization" }
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
        var directoryModel = new DirectoryModel
            { _itemNameFlat = "test", item = "test", subItems = new List<DirectoryModel>() };
       
        var transferModel = new TransferModel
        {
            JsonData = directoryModel,
            UserName = "demo",
            Password = "test"
        };

        var settings = _settings.Value;
        _httpClient.PostAsync<ResultViewModel>(transferModel.JsonData).Returns(new ResultViewModel());

        var result = await _transferService.PostToRecipientAsync(transferModel);

        Assert.LessOrEqual(0, result.Errors.Count());
    }


    [Test]
    public void PostToRecipientAsync_WhenArgumentIsNull_ThrowsNullReferenceException()
    {
        // act & assert
        Assert.ThrowsAsync<NullReferenceException>(() => _transferService.PostToRecipientAsync(null));
    }
}