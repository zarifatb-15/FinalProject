using FluentValidation;
using OnlineAuctionApp.Application.DTOs.Bids;

namespace OnlineAuctionApp.Application.Validators.Bids;

public class BidCreateDtoValidator : AbstractValidator<BidCreateDto>
{
    public BidCreateDtoValidator()
    {
        RuleFor(dto => dto.Amount)
            .GreaterThan(0).WithMessage("Bid amount must be greater than zero.");
    }
}