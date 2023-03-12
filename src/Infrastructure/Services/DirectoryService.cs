using Domain.Entities;
using Infrastructure.Security.Contracts;

namespace Infrastructure.Services;

public abstract class DirectoryService
{
    private static async Task<DirectoryModel> CreateStructureFromDirectoryNode(DirectoryInfo directoryInfo,
        IEncrypter encrypter)
    {
        var node = new DirectoryModel
        {
            _itemNameFlat = directoryInfo.Name,
            item = await encrypter.EncryptAsync(directoryInfo.Name),
            subItems = new List<DirectoryModel>()
        };
        foreach (var directory in directoryInfo.GetDirectories())
            node.subItems.Add(await CreateStructureFromDirectoryNode(directory, encrypter));

        foreach (var file in directoryInfo.GetFiles())
            node.subItems.Add(new DirectoryModel
            {
                _itemNameFlat = file.Name,
                item = await encrypter.EncryptAsync(file.Name)
            });

        return node;
    }

    public static async Task<DirectoryModel> CreateEncyptedDirectoryNode(string fileName, IEncrypter encrypter)
    {
        var root = new DirectoryModel();
        root._itemNameFlat = fileName;
        root.item = await encrypter.EncryptAsync(fileName);
        root.subItems = new List<DirectoryModel>();
        root.subItems.Add(await CreateStructureFromDirectoryNode(new DirectoryInfo(fileName), encrypter));
        return root;
    }

    public static async Task GetDecryptedDirectoryNode(DirectoryModel model, IDecrypter decrypter)
    {
        model.item = await decrypter.DecryptAsync(model.item);

        foreach (var item in model.subItems) await GetDecryptedDirectoryNode(item, decrypter);
    }
}