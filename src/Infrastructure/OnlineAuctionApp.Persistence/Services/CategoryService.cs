using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineAuctionApp.Application.DTOs.Categories;
using OnlineAuctionApp.Application.Interfaces.Services;
using OnlineAuctionApp.Domain.Entities;
using OnlineAuctionApp.Persistence.Contexts;

namespace OnlineAuctionApp.Persistence.Services;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public CategoryService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CategoryReturnDto>> GetAllAsync()
    {
        var categories = await _context.Categories
            .AsNoTracking()
            .ToListAsync();

        return _mapper.Map<List<CategoryReturnDto>>(categories);
    }

    public async Task<CategoryReturnDto> GetByIdAsync(Guid id)
    {
        var category = await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(category => category.Id == id);

        if (category is null)
            throw new InvalidOperationException("Category not found.");

        return _mapper.Map<CategoryReturnDto>(category);
    }

    public async Task<CategoryReturnDto> CreateAsync(CategoryCreateDto dto)
    {
        var exists = await _context.Categories
            .AnyAsync(category => category.Name.ToLower() == dto.Name.ToLower());

        if (exists)
            throw new InvalidOperationException("Category already exists.");

        var category = _mapper.Map<Category>(dto);

        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();

        return _mapper.Map<CategoryReturnDto>(category);
    }

    public async Task<CategoryReturnDto> UpdateAsync(Guid id, CategoryUpdateDto dto)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category is null)
            throw new InvalidOperationException("Category not found.");

        _mapper.Map(dto, category);
        category.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return _mapper.Map<CategoryReturnDto>(category);
    }

    public async Task DeleteAsync(Guid id)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category is null)
            throw new InvalidOperationException("Category not found.");

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }
}