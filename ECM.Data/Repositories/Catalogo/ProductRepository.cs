using ECM.Application.Interfaces.Respository.Catalogo;
using ECM.Domain.Entities.Catalogo;
using ECM.Data.Context;
using Microsoft.EntityFrameworkCore;


namespace ECM.Data.Repositories.Catalogo;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products.Include(p => p.Category).ToListAsync();
    }

    public async Task<Product?> AddAsync(Product entity)
    {
        var result = await _context.Products.AddAsync(entity);
        await _context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<Product> Update(Product entity, int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return null;

        _context.Entry(product).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product?> Disable(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return null;

        product.IsActive = false; // Asumiendo que BaseEntity tiene IsActive como User
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product?> GetBySkuAsync(string sku)
    {
        return await _context.Products.FirstOrDefaultAsync(p => p.SKU == sku);
    }

    public async Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId)
    {
        return await _context.Products
            .Where(p => p.CategoryId == categoryId)
            .ToListAsync();
    }
}