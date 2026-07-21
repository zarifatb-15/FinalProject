using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineAuctionApp.Application.Common.Exceptions;
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
        {
            throw new NotFoundException("Category not found.");
        }

        return _mapper.Map<CategoryReturnDto>(category);
    }

    public async Task<CategoryReturnDto> CreateAsync(CategoryCreateDto dto)
    {
        var name = dto.Name.Trim();
        var description = dto.Description.Trim();

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BadRequestException("Category name is required.");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new BadRequestException("Category description is required.");
        }

        var exists = await _context.Categories
            .AnyAsync(category => category.Name.ToLower() == name.ToLower());

        if (exists)
        {
            throw new ConflictException("Category already exists.");
        }

        var category = new Category
        {
            Name = name,
            Description = description
        };

        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();

        return _mapper.Map<CategoryReturnDto>(category);
    }

    public async Task<CategoryReturnDto> UpdateAsync(Guid id, CategoryUpdateDto dto)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category is null)
        {
            throw new NotFoundException("Category not found.");
        }

        var name = dto.Name.Trim();
        var description = dto.Description.Trim();

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BadRequestException("Category name is required.");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new BadRequestException("Category description is required.");
        }

        var nameExists = await _context.Categories
            .AnyAsync(existingCategory =>
                existingCategory.Id != id &&
                existingCategory.Name.ToLower() == name.ToLower());

        if (nameExists)
        {
            throw new ConflictException("Category already exists.");
        }

        category.Name = name;
        category.Description = description;
        category.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return _mapper.Map<CategoryReturnDto>(category);
    }

    public async Task DeleteAsync(Guid id)
    {
        var category = await _context.Categories
            .Include(category => category.Auctions)
            .FirstOrDefaultAsync(category => category.Id == id);

        if (category is null)
        {
            throw new NotFoundException("Category not found.");
        }

        if (category.Auctions.Any())
        {
            throw new ConflictException(
                "This category cannot be deleted because it has auction listings.");
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }
}