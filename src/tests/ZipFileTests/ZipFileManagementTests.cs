using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Security.Contracts;
using Infrastructure.Security.Cryptor;
using Infrastructure.Services;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace tests.ZipFileTests;

public class ZipFileManagementTests
{
    private IOptions<ApplicationOptions> _options;

    private IEncrypter _encrypter;

    private ZipManagementService _zipManagementService;

    private readonly string zipFileDirectory = "../../../";

    private string zipFilePath = string.Empty;


    [SetUp]
    public void SetUp()
    {
        _options = Options.Create(new ApplicationOptions
        {
            UploadSettings = new UploadSettings { ZipPath = "zipFiles"},
            Security = new Security { CryptoKey = "8137081371813720"}
        });


        zipFilePath = Path.Combine(zipFileDirectory,
           _options.Value.UploadSettings.ZipPath);

        if (!Directory.Exists(zipFilePath))
            Directory.CreateDirectory(zipFilePath);

        _encrypter = new Encrypter(_options);
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

        _zipManagementService = new ZipManagementService(_options, _encrypter);

        var directoryModel = await _zipManagementService.GetSerializedDirectoryStructure(zipFile);

        Assert.NotNull(directoryModel);
    }

    [TearDown]
    public void TearDown()
    {
        Directory.Delete(zipFilePath, true);
    }
}