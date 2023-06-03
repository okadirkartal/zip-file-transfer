
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public sealed class ApplicationOptions
{   [Required]
    public Security Security { get; set; }
    public UploadSettings UploadSettings { get; set; }
    public Storage Storage { get; set; }
    public Credentials Credentials { get; set; }
    [Required]
    public RecipientApi RecipientApi { get; set; }
}

public class Security
{   
    [Required]
    public string Header { get; set; }
    [Required]
    public string CryptoKey { get; set; }
}

public class UploadSettings
{
    public string ZipPath { get; set; }
    public string ExtractedZipPath { get; set; }
}

public class Storage
{
    public string ConnectionString { get; set; }
}

public class  Credentials
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class RecipientApi
{   
    [Required]
    public string BaseUrl { get; set; }
}

