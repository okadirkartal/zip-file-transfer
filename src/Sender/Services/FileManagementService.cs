using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Sender.Models;
using Sender.Services.Contracts;

namespace Sender.Services;

public class FileManagementService : IFileManagementService
{
    private readonly string _contentRootPath;
    private readonly UploadSettings _settings;

    public FileManagementService(IOptions<UploadSettings> settings, IWebHostEnvironment environment)
    {
        _settings = settings.Value;
        _contentRootPath = environment.ContentRootPath;
    }

    public async Task<(bool result, string savedFilePath)> SaveZipFileToLocalFolder(IFormFile file)
    {
        var combinedPath =
            Path.Combine(_contentRootPath, _settings.ZipPath);

        if (!Directory.Exists(combinedPath))
            Directory.CreateDirectory(combinedPath);

        if (file.Length > 0)
        {
            var fullPath = Path.Combine(combinedPath,
                $"{Path.GetFileNameWithoutExtension(file.FileName)}{DateTime.UtcNow:yyyyMMddHmmss}.zip");

            await using (var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
            {
                await file.CopyToAsync(fileStream);
            }

            return (true, fullPath);
        }

        return (false, null);
    }
}