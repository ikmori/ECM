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

    public async Task<Address> GetByIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("El id no puede ser menor o igual a 0", nameof(id));
        }

        var address = await _context.Adresses.FindAsync(id);
        
        if (address == null)
        {
            throw new KeyNotFoundException($"No se encontró la dirección con el ID {id}.");
        }
        
        return address;
    }

    public async Task<IEnumerable<Address>> GetAllAsync()
    {
        var addresses = await _context.Adresses.ToListAsync();
        return addresses;
    }

    public async Task<Address> AddAsync(Address entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "La dirección a agregar no puede ser nula.");
        }

        var address = await _context.Adresses.AddAsync(entity);
        await _context.SaveChangesAsync(); 
        return address.Entity;
    }

    public async Task<Address> Update(Address entity, int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("El id no puede ser menor o igual a 0", nameof(id));
        }

        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "La entidad a actualizar no puede ser nula.");
        }

        var address = await _context.Adresses.FindAsync(id);
        
        if (address == null)
        {
            throw new KeyNotFoundException($"No se encontró la dirección con el ID {id}.");
        }

        _context.Entry(address).CurrentValues.SetValues(entity);
        
        await _context.SaveChangesAsync();
        
        return address;
    }

    public async Task<Address> Disable(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("El id no puede ser menor o igual a 0", nameof(id));
        }

        var address = await _context.Adresses.FindAsync(id);
        
        if (address == null)
        {
            throw new KeyNotFoundException($"No se encontró la dirección con el ID {id}.");
        }

        address.IsActive = false;
        
        await _context.SaveChangesAsync(); 
        
        return address;
    }
}