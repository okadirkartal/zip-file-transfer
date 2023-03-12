
using Domain.Entities;

namespace Infrastructure.Services.Contracts;
public interface IZipManagementService
{
    Task<DirectoryModel> GetSerializedDirectoryStructure(string savedZipFilePath);
}
