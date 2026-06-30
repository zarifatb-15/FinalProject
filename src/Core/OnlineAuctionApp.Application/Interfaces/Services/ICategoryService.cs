using OnlineAuctionApp.Application.DTOs.Categories;
namespace OnlineAuctionApp.Application.Interfaces.Services;

public interface ICategoryService
{
    Task<List<CategoryReturnDto>> GetAllAsync();
    Task<CategoryReturnDto> GetByIdAsync(Guid id);
    Task<CategoryReturnDto> CreateAsync(CategoryCreateDto dto);
    Task<CategoryReturnDto> UpdateAsync(Guid id, CategoryUpdateDto dto);
    Task DeleteAsync(Guid id);
    
}