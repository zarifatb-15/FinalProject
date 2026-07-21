using FluentValidation;
using OnlineAuctionApp.Application.DTOs.Auth;

namespace OnlineAuctionApp.Application.Validators.Auth;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(dto => dto.UsernameOrEmail)
            .NotEmpty().WithMessage("Username or email is required.");

        RuleFor(dto => dto.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}