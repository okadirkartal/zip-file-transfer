using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Sender.Services.Contracts;

namespace Sender.Services
{
    public class FileManagementService : IFileManagementService
    {
        private readonly IConfiguration _configuration;

        private readonly string _contentRootPath;

        public FileManagementService(IConfiguration configuration, IHostingEnvironment environment)
        {
            this._configuration = configuration;
            _contentRootPath = environment.ContentRootPath;
        }

        public async Task<(bool result, string savedFilePath)> SaveZipFileToLocalFolder(IFormFile file)
        {
            var combinedPath =
                Path.Combine(_contentRootPath, _configuration[Core.Common.Constants.UploadPathForZipFiles]);

            if (!Directory.Exists(combinedPath))
                Directory.CreateDirectory(combinedPath);

            if (file.Length > 0)
            {
                string fullPath = Path.Combine(combinedPath,
                    $"{Path.GetFileNameWithoutExtension(file.FileName)}{DateTime.UtcNow:yyyyMMddHmmss}.zip");

                using (var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                {
                    await file.CopyToAsync(fileStream);
                }

                return (true, fullPath);
            }

            return (false, null);
        }
    }
}