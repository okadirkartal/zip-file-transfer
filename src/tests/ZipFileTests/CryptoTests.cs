using System.Threading.Tasks;
using Infrastructure.Security;
using Infrastructure.Security.Contracts;
using Infrastructure.Security.Cryptor;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using NUnit.Framework;

namespace tests.ZipFileTests;

public class EncryptAndDecryptTests
{
    private IConfiguration _configuration;

    private IDecrypter _decrypter;
    private IEncrypter _encrypter;

    [SetUp]
    public void SetUp()
    {
        _configuration = Substitute.For<IConfiguration>();

        _configuration["Security:CryptoKey"].Returns("8137081371813720");

        _encrypter = new Encrypter(_configuration);

        _decrypter = new Decrypter(_configuration);
    }

    [TestCase("kartal")]
    [TestCase("810123481")]
    [TestCase("!@EST@N!A")]
    public async Task SetSomeData_WhenCryptoIsCorrect_ReturnsTrue(string data)
    {
        var encryptedData = await _encrypter.EncryptAsync(data);

        var decryptedData = await _decrypter.DecryptAsync(encryptedData);

        Assert.AreEqual(data, decryptedData);
    }


    [TestCase("kartal")]
    [TestCase("810123481")]
    [TestCase("!@EST@N!A")]
    public async Task SetSomeData_WhenCryptoKeyIsMissing_ReturnsTrue(string data)
    {
        var encryptedData = await _encrypter.EncryptAsync(data);

        var decryptedData = await _decrypter.DecryptAsync(encryptedData);

        Assert.AreEqual(data, decryptedData);
    }
}