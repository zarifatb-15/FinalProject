using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineAuctionApp.Application.DTOs.Categories;
using OnlineAuctionApp.Application.Interfaces.Services;

namespace OnlineAuctionApp.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : BaseApiController
{
    private readonly ICategoryService _categoryService;
    private readonly IValidator<CategoryCreateDto> _categoryCreateValidator;
    private readonly IValidator<CategoryUpdateDto> _categoryUpdateValidator;

    public CategoriesController(
        ICategoryService categoryService,
        IValidator<CategoryCreateDto> categoryCreateValidator,
        IValidator<CategoryUpdateDto> categoryUpdateValidator)
    {
        _categoryService = categoryService;
        _categoryCreateValidator = categoryCreateValidator;
        _categoryUpdateValidator = categoryUpdateValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _categoryService.GetAllAsync();
        return ApiSuccess(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        return ApiSuccess(category);
    }

    [Authorize(Roles = "Seller,Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CategoryCreateDto dto)
    {
        var validationError = await ValidateRequestAsync(dto, _categoryCreateValidator);

        if (validationError is not null)
        {
            return validationError;
        }

        var category = await _categoryService.CreateAsync(dto);
        return ApiCreated(nameof(GetById), new { id = category.Id }, category);
    }

    [Authorize(Roles = "Seller,Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CategoryUpdateDto dto)
    {
        var validationError = await ValidateRequestAsync(dto, _categoryUpdateValidator);

        if (validationError is not null)
        {
            return validationError;
        }

        var category = await _categoryService.UpdateAsync(id, dto);
        return ApiSuccess(category);
    }

    [Authorize(Roles = "Seller,Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _categoryService.DeleteAsync(id);
        return ApiDeleted("Category deleted successfully.");
    }
}