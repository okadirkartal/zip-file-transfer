using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Sender.Services.Contracts;

public interface IFileManagementService
{
    Task<(bool result, string savedFilePath)> SaveZipFileToLocalFolder(IFormFile file);
}