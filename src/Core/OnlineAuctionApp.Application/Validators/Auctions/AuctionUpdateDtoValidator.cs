using FluentValidation;
using OnlineAuctionApp.Application.DTOs.Auctions;

namespace OnlineAuctionApp.Application.Validators.Auctions;

public class AuctionUpdateDtoValidator : AbstractValidator<AuctionUpdateDto>
{
    public AuctionUpdateDtoValidator()
    {
        RuleFor(auction => auction.Title)
            .NotEmpty()
            .WithMessage("Auction title is required.")
            .MaximumLength(150)
            .WithMessage("Auction title cannot exceed 150 characters.");

        RuleFor(auction => auction.Description)
            .NotEmpty()
            .WithMessage("Auction description is required.")
            .MaximumLength(2000)
            .WithMessage("Auction description cannot exceed 2000 characters.");

        RuleFor(auction => auction.StartingPrice)
            .GreaterThan(0)
            .WithMessage("Starting price must be greater than zero.");

        RuleFor(auction => auction.EndTime)
            .Must(endTime => endTime > DateTime.UtcNow)
            .WithMessage("End time must be in the future.");

        RuleFor(auction => auction.CategoryId)
            .NotEmpty()
            .WithMessage("Category is required.");
    }
}