
using Infrastructure.Security.Contracts;
using System.Text.Json;
using System.Text.Json.Serialization;
using Domain.Entities;

namespace Infrastructure.Services;
public class DirectoryService
{
    private static async Task<DirectoryModel> CreateStructureFromDirectoryNode(DirectoryInfo directoryInfo, IEncrypter encrypter)
    {
        var node = new DirectoryModel();
        node._itemNameFlat = directoryInfo.Name;
        node.item = await encrypter.EncryptAsync(directoryInfo.Name);
        node.subItems = new List<DirectoryModel>();
        foreach (var directory in directoryInfo.GetDirectories())
        {
            node.subItems.Add(await CreateStructureFromDirectoryNode(directory, encrypter));
        }

        foreach (var file in directoryInfo.GetFiles())
        {
            var subDirectoryModel = new DirectoryModel();
            subDirectoryModel._itemNameFlat = file.Name;
            subDirectoryModel.item = await encrypter.EncryptAsync(file.Name);
            node.subItems.Add(subDirectoryModel);
        }

        return node;
    }

    public static async Task<DirectoryModel> CreateStructureFromDirectoryNode(string fileName, IEncrypter encrypter)
    {
        DirectoryModel root = new DirectoryModel();
        root._itemNameFlat = fileName;
        root.item = await encrypter.EncryptAsync(fileName);
        root.subItems = new List<DirectoryModel>();
        root.subItems.Add(await CreateStructureFromDirectoryNode(new DirectoryInfo(fileName), encrypter));
        return root;
    }

    public static async Task<string> GetDecryptedDirectoryNode(string jsonData, IDecrypter decrypter)
    {
        var directoryModel = JsonSerializer.Deserialize<DirectoryModel>(jsonData,new JsonSerializerOptions{ DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull});

        await GetDecryptedDirectoryNode(directoryModel, decrypter);

        return JsonSerializer.Serialize(directoryModel);
    }

    private static async Task GetDecryptedDirectoryNode(DirectoryModel model, IDecrypter decrypter)
    {
        model.item = await decrypter.DecryptAsync(model.item);

        foreach (var item in model.subItems)
        {
            await GetDecryptedDirectoryNode(item, decrypter);
        }
    }
}
