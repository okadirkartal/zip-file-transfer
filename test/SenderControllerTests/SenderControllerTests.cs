using System;
using System.Threading.Tasks;
using Core.ZipManagementService.Contracts;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using Security.Cryptor.Contracts;
using Sender.Controllers;
using Sender.Services.Contracts;

namespace RepositoryTests
{
    public class SenderControllerTests
    {
        private readonly SenderController _senderController;

        private readonly ITransferService _transferService;

        private readonly IZipManagementService _zipManagementService;

        private readonly IFileManagementService _fileManagementService;

        private readonly IEncrypter _encrypter;

        private readonly IFormFile _file;

        public SenderControllerTests()
        {
            _transferService = Substitute.For<ITransferService>();

            _zipManagementService = Substitute.For<IZipManagementService>();

            _fileManagementService = Substitute.For<IFileManagementService>();

            _encrypter = Substitute.For<IEncrypter>();

            _file = Substitute.For<IFormFile>();

            _senderController = new SenderController(_fileManagementService, _zipManagementService, _encrypter,
                _transferService);
        }

        [Test]
        public void CreateConstructor_WhenArgumentIsNull_ThrowsArgumentNullException()
        {
            // act & assert
            Assert.Throws<ArgumentNullException>(() => new SenderController(null, null, null, null));
        }


        [Test]
        public async Task PostToRecipientAsync_WhenArgumentIsNull_ReturnsBadRequest()
        {
            // act
            var result = await _senderController.Index(null);

            // assert
            result.Should().BeOfType(typeof(BadRequestResult));
        }
    }
}