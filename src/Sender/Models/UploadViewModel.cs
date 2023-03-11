using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Sender.Attributes;

namespace Sender.Models;
public class UploadViewModel
{
    [Required] public string UserName { get; set; }

    [Required] public string Password { get; set; }


    [Required(ErrorMessage = "Please select a zip file")]
    [DisplayName("Select a zip file")]
    [ValidateZip]
    public IFormFile File { get; set; }
}