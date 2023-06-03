using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Security;
using Infrastructure.Security.Contracts;
using Infrastructure.Security.Cryptor; 
using Microsoft.Extensions.Options; 
using NUnit.Framework;

namespace tests.ZipFileTests;

public class EncryptAndDecryptTests
{
    private IOptions<ApplicationOptions> _options;

    private IDecrypter _decrypter;
    private IEncrypter _encrypter;

    [SetUp]
    public void SetUp()
    {
        _options = Options.Create(new ApplicationOptions
        {
            Security = new Security { CryptoKey = "8137081371813720" }
        });
       
        _encrypter = new Encrypter(_options);

        _decrypter = new Decrypter(_options);
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