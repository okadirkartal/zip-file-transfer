using System.IO.Compression;
using Domain.Entities;
using Infrastructure.Security.Contracts;
using Infrastructure.Services.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;

public class ZipManagementService : IZipManagementService
{
    private readonly UploadSettings _uploadSettings;
    private readonly IEncrypter _encrypter;

    public ZipManagementService(IOptions<ApplicationOptions> options,
        IEncrypter encrypter)
    {
        _uploadSettings = options.Value.UploadSettings;
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
        var saveablePath = Path.Combine(_uploadSettings.ZipPath ?? string.Empty,
            Path.GetFileNameWithoutExtension(savedZipFilePath));

        zipFile.ExtractToDirectory(saveablePath, true);
        return saveablePath;
    }
}