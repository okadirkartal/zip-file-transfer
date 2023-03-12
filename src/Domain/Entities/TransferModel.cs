namespace Domain.Entities; 
public class TransferModel : UserModel
{
    public string Id { get; set; }

    public DirectoryModel JsonData { get; set; }
}
