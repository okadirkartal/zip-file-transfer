
using System.ComponentModel.DataAnnotations;

namespace Sender.Models;
public class UploadSettings
{

    [Required]
    public string ZipPath { get; set; }
    [Required]
    public string ExtractedZipPath { get; set; }
}