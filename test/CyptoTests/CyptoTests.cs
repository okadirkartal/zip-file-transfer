using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using NUnit.Framework;
using Security.Cryptor;
using Security.Cryptor.Contracts;


namespace CyptoTests
{
    public class EncryptAndDecryptTests
    {
        private IEncrypter _encrypter;

        private IDecrypter _decrypter;

        private IConfiguration _configuration;

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
            string encryptedData = await _encrypter.EncryptAsync(data);

            string decryptedData = await _decrypter.DecryptAsync(encryptedData);

            Assert.AreEqual(data, decryptedData);
        }


        [TestCase("kartal")]
        [TestCase("810123481")]
        [TestCase("!@EST@N!A")]
        public async Task SetSomeData_WhenCryptoKeyIsMissing_ReturnsTrue(string data)
        {
            string encryptedData = await _encrypter.EncryptAsync(data);

            string decryptedData = await _decrypter.DecryptAsync(encryptedData);

            Assert.AreEqual(data, decryptedData);
        }
    }
}