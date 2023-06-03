namespace Domain.Entities;

public sealed class DirectoryModel
{
    [NonSerialized] public string _itemNameFlat;

    public string item { get; set; }

    public List<DirectoryModel> subItems { get; set; }
}