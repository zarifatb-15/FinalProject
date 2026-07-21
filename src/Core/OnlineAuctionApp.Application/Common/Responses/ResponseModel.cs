namespace OnlineAuctionApp.Application.Common.Responses;

public class ResponseModel<T>
{
    public bool IsSuccess { get; set; }
    public int StatusCode { get; set; }
    public List<string>? Errors { get; set; }
    public T? Data { get; set; }
}