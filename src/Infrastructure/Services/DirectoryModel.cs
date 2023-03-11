
using Infrastructure.Security.Contracts;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.Services;
public class DirectoryModel
{
    [NonSerialized] public string _itemNameFlat;

    public string item { get; set; }

    public List<DirectoryModel> subItems { get; set; }

    public DirectoryModel(string itemNameFlat, string item)
    {
        this._itemNameFlat = itemNameFlat;

        this.item = item;

        subItems = new List<DirectoryModel>();
    }

    private static async Task<DirectoryModel> CreateStructureFromDirectoryNode(DirectoryInfo directoryInfo, IEncrypter encrypter)
    {
        var node = new DirectoryModel(directoryInfo.Name, await encrypter.EncryptAsync(directoryInfo.Name));

        foreach (var directory in directoryInfo.GetDirectories())
        {
            node.subItems.Add(await CreateStructureFromDirectoryNode(directory, encrypter));
        }

        foreach (var file in directoryInfo.GetFiles())
        {
            node.subItems.Add(new DirectoryModel(file.Name, await encrypter.EncryptAsync(file.Name)));
        }

        return node;
    }

    public static async Task<DirectoryModel> CreateStructureFromDirectoryNode(string fileName, IEncrypter encrypter)
    {
        DirectoryModel root = new DirectoryModel(fileName, await encrypter.EncryptAsync(fileName));
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
