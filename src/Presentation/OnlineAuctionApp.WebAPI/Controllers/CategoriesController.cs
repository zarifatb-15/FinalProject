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

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
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
        try
        {
            var category = await _categoryService.GetByIdAsync(id);
            return ApiSuccess(category);
        }
        catch (InvalidOperationException ex)
        {
            return ApiNotFound(ex.Message);
        }
    }

    [Authorize(Roles = "Seller,Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(CategoryCreateDto dto)
    {
        try
        {
            var category = await _categoryService.CreateAsync(dto);
            return ApiCreated(nameof(GetById), new { id = category.Id }, category);
        }
        catch (InvalidOperationException ex)
        {
            return ApiBadRequest(ex.Message);
        }
    }

    [Authorize(Roles = "Seller,Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, CategoryUpdateDto dto)
    {
        try
        {
            var category = await _categoryService.UpdateAsync(id, dto);
            return ApiSuccess(category);
        }
        catch (InvalidOperationException ex)
        {
            if (ex.Message == "Category not found.")
            {
                return ApiNotFound(ex.Message);
            }

            return ApiBadRequest(ex.Message);
        }
    }

    [Authorize(Roles = "Seller,Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _categoryService.DeleteAsync(id);
            return ApiDeleted("Category deleted successfully.");
        }
        catch (InvalidOperationException ex)
        {
            if (ex.Message == "Category not found.")
            {
                return ApiNotFound(ex.Message);
            }

            return ApiConflict(ex.Message);
        }
    }
}