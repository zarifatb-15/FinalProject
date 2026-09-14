namespace OnlineAuctionApp.Application.Interfaces.Services;

public interface IAppMessageService
{
    string Get(string key);
    string Get(string key, params object[] args);
}