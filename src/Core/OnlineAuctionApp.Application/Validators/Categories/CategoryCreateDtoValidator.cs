using FluentValidation;
using OnlineAuctionApp.Application.DTOs.Categories;

namespace OnlineAuctionApp.Application.Validators.Categories;

public class CategoryCreateDtoValidator : AbstractValidator<CategoryCreateDto>
{
    public CategoryCreateDtoValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty().WithMessage("Category name is required.")
            .MaximumLength(100).WithMessage("Category name cannot exceed 100 characters.");

        RuleFor(dto => dto.Description)
            .NotEmpty().WithMessage("Category description is required.")
            .MaximumLength(500).WithMessage("Category description cannot exceed 500 characters.");
    }
}