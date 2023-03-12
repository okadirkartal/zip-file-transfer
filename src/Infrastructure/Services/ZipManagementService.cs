
using System.IO.Compression;
using Infrastructure.Security.Contracts;
using Infrastructure.Services.Contracts;
using System.Text.Json;
using System.Text.Json.Serialization;
using Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public class ZipManagementService : IZipManagementService
    {
        private readonly IEncrypter _encrypter;

        private readonly IConfiguration _configuration;

        public ZipManagementService(IConfiguration configuration,
            IEncrypter encrypter)
        {
            this._configuration = configuration;
            this._encrypter = encrypter;
        }

        private string UnzipFileToGivenPath(string savedZipFilePath)
        {
            using ZipArchive zipFile = ZipFile.OpenRead(savedZipFilePath);
            string saveablePath = Path.Combine(_configuration["UploadSettings:ZipPath"] ?? string.Empty,
                Path.GetFileNameWithoutExtension(savedZipFilePath));

            zipFile.ExtractToDirectory(saveablePath, true);
            return saveablePath;
        }

        public async Task<DirectoryModel> GetSerializedDirectoryStructure(string savedZipFilePath)
        {
            return await DirectoryService.CreateEncyptedDirectoryNode(UnzipFileToGivenPath(savedZipFilePath),
                    _encrypter); 
        }
    }
}