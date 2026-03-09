using ECM.Application.Interfaces.Respository.Identidades;
using ECM.Data.Context;
using ECM.Domain.Entities.Identidades;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECM.Data.Repositories.Identidades;

public class UserRepository : IUserRepository
{
    /// <summary>
    /// Creación del repositorio Users el cual se encarga de registrar los usuarios y
    /// posteriormente también servirá para loguearlos en la página de inicio.
    /// </summary>
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User?> AddAsync(User entity)
    {
        var user = await _context.Users.AddAsync(entity);
        await _context.SaveChangesAsync();
        return user.Entity;
    }

    public async Task<User?> Update(User entity, int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null) return null;
        
        _context.Entry(user).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<User?> Disable(int id)
    {
        var user = await _context.Users.FindAsync(id);
        
        if (user == null) return null;

        user.IsActive = false;
        await _context.SaveChangesAsync(); 
        
        return user;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        // Este método estaba perfecto
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetUserWithAddressesAsync(int userId)
    {
        return await _context.Users
            .Include(u => u.Addresses) 
            .FirstOrDefaultAsync(u => u.Id == userId);
    }
}