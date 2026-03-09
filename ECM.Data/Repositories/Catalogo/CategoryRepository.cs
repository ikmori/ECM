

using ECM.Application.Interfaces.Respository.Catalogo;
using ECM.Data.Context;
using ECM.Domain.Entities.Catalogo;
using Microsoft.EntityFrameworkCore;

namespace ECM.Data.Repositories.Catalogo;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _context.Categories
            .Include(c => c.ParentCategory)
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<Category?> AddAsync(Category entity)
    {
        var result = await _context.Categories.AddAsync(entity);
        await _context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<Category?> Update(Category entity, int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return null;

        _context.Entry(category).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<Category?> Disable(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return null;

        category.IsActive = false; 
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<IEnumerable<Category>> GetSubCategoriesAsync(int parentId)
    {
        return await _context.Categories
            .Where(c => c.ParentCategoryId == parentId)
            .ToListAsync();
    }

   
}