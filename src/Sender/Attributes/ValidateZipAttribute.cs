using System.Collections;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Sender.Attributes;

public class ValidateZipAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        const int maxContentLength = 1024 * 1024 * 10; //Max 10 MB file


        var allowedFileExtensions = new[] { ".zip" };

        if (!(value is IFormFile file)) return false;

        if (!((IList)allowedFileExtensions).Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))
                .ToLower()))
        {
            ErrorMessage = "Please upload Your file of type: " +
                           string.Join(", ", allowedFileExtensions);
            return false;
        }

        if (file.Length > maxContentLength)
        {
            ErrorMessage = "Your file is too large, maximum allowed size is : "
                           + maxContentLength / 1024 + "MB";
            return false;
        }

        return true;
    }
}