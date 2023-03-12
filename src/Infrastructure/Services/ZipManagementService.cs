
using System.IO.Compression;
using Infrastructure.Security.Contracts;
using Infrastructure.Services.Contracts;
using System.Text.Json;
using System.Text.Json.Serialization;
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
            using (ZipArchive zipFile = ZipFile.OpenRead(savedZipFilePath))
            {
                string saveablePath = Path.Combine(_configuration["UploadSettings:ZipPath"],
                    Path.GetFileNameWithoutExtension(savedZipFilePath));

                zipFile.ExtractToDirectory(saveablePath, true);
                return saveablePath;
            }
        }

        public async Task<string> GetSerializedDirectoryStructure(string savedZipFilePath)
        {
            var result =
                await DirectoryService.CreateStructureFromDirectoryNode(UnzipFileToGivenPath(savedZipFilePath),
                    _encrypter);

            return JsonSerializer.Serialize(result,new JsonSerializerOptions() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull} );
        }
    }
}