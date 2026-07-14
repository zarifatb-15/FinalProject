namespace OnlineAuctionApp.Application.DTOs.Auctions;

public class AuctionFilterDto
{
    public Guid? CategoryId { get; set; }
    public string? Search { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
}