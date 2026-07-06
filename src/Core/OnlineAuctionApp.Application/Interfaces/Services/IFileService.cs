namespace OnlineAuctionApp.Application.Interfaces.Services;

public interface IFileService
{
    Task<string> SaveFileAsync(Stream fileStream, string originalFileName, string folderName);
    void DeleteFile(string relativePath);
}