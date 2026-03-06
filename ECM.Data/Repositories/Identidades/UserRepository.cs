using ECM.Application.Interfaces.Respository.Identidades;
using ECM.Data.Context;
using ECM.Domain.Entities.Identidades;
using Microsoft.EntityFrameworkCore;

namespace ECM.Data.Repositories.Identidades;

public class UserRepository : IUserRepository
{
    /// <summary>
    /// Creacion del repositorio Users el cual se encarga de registrar los usuarios y
    /// posteriormente tambien servira para logearlos en la pagina de inicio
    /// </summary>
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetByIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("El id no puede ser menor o igual a 0", nameof(id));
        }

        var user = await _context.Users.FindAsync(id);
        
        if (user == null)
        {
            throw new KeyNotFoundException($"El usuario con el id {id} no existe");
        }
        return user;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        var users = await _context.Users.ToListAsync();
        return users;
    }

    public async Task<User> AddAsync(User entity)
    {
        var user = await _context.Users.AddAsync(entity);
        await _context.SaveChangesAsync();
        return user.Entity;
    }

    public async Task<User> Update(User entity, int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("El id no puede ser menor o igual a 0", nameof(id));
        }

        // CORRECCIÓN 1: Lo que puede ser nulo aquí es la 'entity' que te mandan, no el 'int id'
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "La entidad de usuario no puede ser nula");
        }

        var user = await _context.Users.FindAsync(id);
        
        if (user == null)
        {
            throw new KeyNotFoundException($"El usuario con id {id} no existe");
        }
        
        _context.Entry(user).CurrentValues.SetValues(entity);

        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<User> Disable(int id)
    {
        var user = await _context.Users.FindAsync(id);
        
        if (user == null)
        {
            throw new KeyNotFoundException($"No se encontró el usuario con el ID {id}.");
        }

        user.IsActive = false;
        
        await _context.SaveChangesAsync(); 
        
        return user;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        return user;
    }

    public async Task<User?> GetUserWithAddressesAsync(int userId)
    {
        if (userId <= 0)
        {
            throw new ArgumentException("El id no puede ser menor o igual a 0", nameof(userId));
        }
        
        var user = await _context.Users
            .Include(u => u.Addresses) 
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            throw new KeyNotFoundException($"El usuario con id {userId} no existe.");
        }

        return user;
    }
}