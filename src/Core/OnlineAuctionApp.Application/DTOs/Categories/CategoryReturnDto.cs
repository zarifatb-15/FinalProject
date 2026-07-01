namespace OnlineAuctionApp.Application.DTOs.Categories;

public class CategoryReturnDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}