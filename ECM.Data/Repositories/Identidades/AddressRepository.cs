using ECM.Application.Interfaces.Respository.Identidades;
using ECM.Data.Context;
using ECM.Domain.Entities.Identidades;
using Microsoft.EntityFrameworkCore;

namespace ECM.Data.Repositories.Identidades;

public class AddressRepository : IAddressRepository
{
    /// <summary>
    /// Creacion del repositorio de addresses que guarda las direcciones de los usuarios,
    /// las cuales son utilizadas en las facturas y etc.
    /// </summary>
    private readonly AppDbContext _context;

    public AddressRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Address?> GetByIdAsync(int id)
    {
        return await _context.Adresses.FindAsync(id);
    }

    public async Task<IEnumerable<Address>> GetAllAsync()
    {
        return await _context.Adresses.ToListAsync();
    }

    public async Task<Address?> AddAsync(Address entity)
    {
        var address = await _context.Adresses.AddAsync(entity);
        await _context.SaveChangesAsync(); 
        return address.Entity;
    }

    public async Task<Address?> Update(Address entity, int id)
    {
        var address = await _context.Adresses.FindAsync(id);
        
        if (address == null) return null;

        _context.Entry(address).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync();
        
        return address;
    }

    public async Task<Address?> Disable(int id)
    {
        var address = await _context.Adresses.FindAsync(id);
        
        if (address == null) return null;

        address.IsActive = false;
        await _context.SaveChangesAsync(); 
        
        return address;
    }
}