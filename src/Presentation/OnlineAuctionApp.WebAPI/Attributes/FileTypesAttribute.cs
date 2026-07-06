using System.ComponentModel.DataAnnotations;

namespace OnlineAuctionApp.WebAPI.Attributes;

public class FileTypesAttribute : ValidationAttribute
{
    private readonly string[] _allowedTypes;

    public FileTypesAttribute(params string[] allowedTypes)
    {
        _allowedTypes = allowedTypes;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var files = new List<IFormFile>();

        if (value is List<IFormFile> fileList)
        {
            files = fileList;
        }

        if (value is IFormFile singleFile)
        {
            files.Add(singleFile);
        }

        foreach (var item in files)
        {
            if (!_allowedTypes.Contains(item.ContentType))
            {
                return new ValidationResult(
                    $"File type {item.ContentType} is not allowed. Allowed types are: {string.Join(", ", _allowedTypes)}"
                );
            }
        }

        return ValidationResult.Success;
    }
}