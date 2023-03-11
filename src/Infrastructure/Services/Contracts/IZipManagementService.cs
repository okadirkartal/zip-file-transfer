
namespace Infrastructure.Services.Contracts;
public interface IZipManagementService
{
    Task<string> GetSerializedDirectoryStructure(string savedZipFilePath);
}
