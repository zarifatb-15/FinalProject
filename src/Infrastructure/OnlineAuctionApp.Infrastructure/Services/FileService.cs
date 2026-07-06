using Microsoft.Extensions.Hosting;
using OnlineAuctionApp.Application.Interfaces.Services;

namespace OnlineAuctionApp.Infrastructure.Services;

public class FileService : IFileService
{
    private readonly IHostEnvironment _environment;

    public FileService(IHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string> SaveFileAsync(Stream fileStream, string originalFileName, string folderName)
    {
        var webRootPath = Path.Combine(_environment.ContentRootPath, "wwwroot");
        var uploadPath = Path.Combine(webRootPath, "uploads", folderName);

        Directory.CreateDirectory(uploadPath);

        var extension = Path.GetExtension(originalFileName);
        var fileName = $"{Guid.NewGuid()}{extension}";
        var fullPath = Path.Combine(uploadPath, fileName);

        await using var outputStream = new FileStream(fullPath, FileMode.Create);
        await fileStream.CopyToAsync(outputStream);

        return $"/uploads/{folderName}/{fileName}";
    }

    public void DeleteFile(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
            return;

        var webRootPath = Path.Combine(_environment.ContentRootPath, "wwwroot");
        var cleanPath = relativePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString());
        var fullPath = Path.Combine(webRootPath, cleanPath);

        if (File.Exists(fullPath))
            File.Delete(fullPath);
    }
}