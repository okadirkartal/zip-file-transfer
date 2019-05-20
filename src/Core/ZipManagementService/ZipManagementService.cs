using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Security.Cryptor.Contracts;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Core.ZipManagementService.Contracts;

namespace Core.ZipManagementService
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
                string saveablePath = Path.Combine(_configuration[Common.Constants.UploadPathForZipFiles],
                    Path.GetFileNameWithoutExtension(savedZipFilePath));

                zipFile.ExtractToDirectory(saveablePath, true);
                return saveablePath;
            }
        }

        public async Task<string> GetSerializedDirectoryStructure(string savedZipFilePath)
        {
            var result =
                await DirectoryModel.CreateStructureFromDirectoryNode(UnzipFileToGivenPath(savedZipFilePath),
                    _encrypter);

            return JsonConvert.SerializeObject(result,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                });
        }
    }
}