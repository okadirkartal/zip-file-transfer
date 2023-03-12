using System.IO.Compression;
using Domain.Entities;
using Infrastructure.Security.Contracts;
using Infrastructure.Services.Contracts;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class ZipManagementService : IZipManagementService
{
    private readonly IConfiguration _configuration;
    private readonly IEncrypter _encrypter;

    public ZipManagementService(IConfiguration configuration,
        IEncrypter encrypter)
    {
        _configuration = configuration;
        _encrypter = encrypter;
    }

    public async Task<DirectoryModel> GetSerializedDirectoryStructure(string savedZipFilePath)
    {
        return await DirectoryService.CreateEncyptedDirectoryNode(UnzipFileToGivenPath(savedZipFilePath),
            _encrypter);
    }

    private string UnzipFileToGivenPath(string savedZipFilePath)
    {
        using var zipFile = ZipFile.OpenRead(savedZipFilePath);
        var saveablePath = Path.Combine(_configuration["UploadSettings:ZipPath"] ?? string.Empty,
            Path.GetFileNameWithoutExtension(savedZipFilePath));

        zipFile.ExtractToDirectory(saveablePath, true);
        return saveablePath;
    }
}