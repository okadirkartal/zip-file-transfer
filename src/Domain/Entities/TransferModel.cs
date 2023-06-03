namespace Domain.Entities;

public sealed class TransferModel : UserModel
{
    public string Id { get; set; }

    public DirectoryModel JsonData { get; set; }
}