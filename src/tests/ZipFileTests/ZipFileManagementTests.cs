using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Infrastructure.Security.Contracts;
using Infrastructure.Security.Cryptor;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using NUnit.Framework;

namespace tests.ZipFileTests;

public class ZipFileManagementTests
{
    private IConfiguration _configuration;

    private IEncrypter _encrypter;

    private ZipManagementService _zipManagementService;

    private readonly string zipFileDirectory = "../../../";

    private string zipFilePath = string.Empty;


    [SetUp]
    public void SetUp()
    {
        _configuration = Substitute.For<IConfiguration>();

        _configuration["UploadPaths:ZipPath"].Returns("zipFiles");
        _configuration["Security:CryptoKey"].Returns("8137081371813720");

        zipFilePath = Path.Combine(zipFileDirectory,
            _configuration["UploadPaths:ZipPath"]);

        if (!Directory.Exists(zipFilePath))
            Directory.CreateDirectory(zipFilePath);

        _encrypter = new Encrypter(_configuration);
    }

    [Test]
    public async Task GetSerializedDirectoryStructure_WhenIsValidObject_ReturnsTrue()
    {
        //provide the path and name for the zip file to create


        var zipFile = zipFilePath + "test.zip";


        using (var memoryStream = new MemoryStream())
        {
            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                var demoFile = archive.CreateEntry("sample.txt");

                using (var entryStream = demoFile.Open())
                using (var streamWriter = new StreamWriter(entryStream))
                {
                    streamWriter.Write("ESTONIA");
                }
            }

            using (var fileStream = new FileStream(zipFile, FileMode.Create))
            {
                memoryStream.Seek(0, SeekOrigin.Begin);
                memoryStream.CopyTo(fileStream);
            }
        }

        _zipManagementService = new ZipManagementService(_configuration, _encrypter);

        var directoryModel = await _zipManagementService.GetSerializedDirectoryStructure(zipFile);

        Assert.NotNull(directoryModel);
    }

    [TearDown]
    public void TearDown()
    {
        Directory.Delete(zipFilePath, true);
    }
}