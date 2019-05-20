using System.Threading.Tasks;

namespace Core.ZipManagementService.Contracts
{
    public interface IZipManagementService
    {
        Task<string> GetSerializedDirectoryStructure(string savedZipFilePath);
    }
}