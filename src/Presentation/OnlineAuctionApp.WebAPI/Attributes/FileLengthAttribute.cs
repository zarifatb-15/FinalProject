using System.ComponentModel.DataAnnotations;

namespace OnlineAuctionApp.WebAPI.Attributes;

public class FileLengthAttribute : ValidationAttribute
{
    public int Length { get; set; }

    public FileLengthAttribute(int length)
    {
        Length = length;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var files = new List<IFormFile>();

        if (value is IFormFile singleFile)
        {
            files.Add(singleFile);
        }
        else if (value is List<IFormFile> fileList)
        {
            files = fileList;
        }

        foreach (var item in files)
        {
            if (item.Length > Length * 1024 * 1024)
            {
                return new ValidationResult($"File size must be less than {Length}MB.");
            }
        }

        return ValidationResult.Success;
    }
}