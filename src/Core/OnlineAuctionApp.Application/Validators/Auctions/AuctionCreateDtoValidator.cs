using FluentValidation;
using OnlineAuctionApp.Application.DTOs.Auctions;

namespace OnlineAuctionApp.Application.Validators.Auctions;

public class AuctionCreateDtoValidator : AbstractValidator<AuctionCreateDto>
{
    public AuctionCreateDtoValidator()
    {
        RuleFor(dto => dto.Title)
            .NotEmpty().WithMessage("Auction title is required.")
            .MaximumLength(150).WithMessage("Auction title cannot exceed 150 characters.");

        RuleFor(dto => dto.Description)
            .NotEmpty().WithMessage("Auction description is required.")
            .MaximumLength(2000).WithMessage("Auction description cannot exceed 2000 characters.");

        RuleFor(dto => dto.StartingPrice)
            .GreaterThan(0).WithMessage("Starting price must be greater than zero.");

        RuleFor(dto => dto.EndTime)
            .Must(endTime => endTime > DateTime.UtcNow)
            .WithMessage("End time must be in the future.");

        RuleFor(dto => dto.CategoryId)
            .NotEmpty().WithMessage("Category is required.");
    }
}