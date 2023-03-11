namespace Domain.Shared;

public class SharedSettings
{
    public Security Security { get; set; }
    public RecipientApi RecipientApi { get; set; }
}

public class Security
{
    public string Header { get; set; }

    public string CryptoKey { get; set; }
}

public class RecipientApi
{
    public string BaseUrl { get; set; }
}