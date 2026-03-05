using ECM.Application.Interfaces.Respository.Identidades;
using ECM.Application.Interfaces.ServicesInterfaces.IdentidadesServices;
using ECM.Domain.Common.Enums;
using ECM.Domain.Entities.Identidades;

namespace ECM.Application.Services.Identidades;

public class UserService : IUserService
{
    /// <summary>
    /// aqui hay dos servicios muy similares pero con objetivos diferentes que son register y createAsync,
    /// register funcionara para crear usuarios comunes y createAsync para crear usuarios con roles diferentes
    /// </summary>
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    // --- MÉTODOS DEL CRUD BASE ---

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _userRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _userRepository.GetAllAsync();
    }

    public async Task CreateAsync(User entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "El usuario no puede ser nulo.");
        }
        
        if (string.IsNullOrWhiteSpace(entity.Email))
        {
            throw new ArgumentException("El correo electrónico es obligatorio.", nameof(entity.Email));
        }

        // 3. Validar que el correo no exista ya en la base de datos
        var existingUser = await _userRepository.GetByEmailAsync(entity.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("Ya existe un usuario registrado con este correo electrónico.");
        }
        
        await _userRepository.AddAsync(entity);
    }

    public async Task UpdateAsync(User entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "El usuario no puede ser nulo.");
        }

        if (entity.Id <= 0)
        {
            throw new ArgumentException("El ID del usuario no es válido.", nameof(entity.Id));
        }

        if (string.IsNullOrWhiteSpace(entity.Email))
        {
            throw new ArgumentException("El correo electrónico es obligatorio.", nameof(entity.Email));
        }

        var existingUserWithEmail = await _userRepository.GetByEmailAsync(entity.Email);

        if (existingUserWithEmail != null && existingUserWithEmail.Id != entity.Id)
        {
            throw new InvalidOperationException("El correo electrónico que intentas usar ya le pertenece a otro usuario.");
        }

        await _userRepository.Update(entity, entity.Id);
    }

    public async Task DeleteAsync(int id)
    {
        await _userRepository.Disable(id);
    }

    // --- MÉTODOS DE REGLAS DE NEGOCIO ---

    public async Task<User> Register(User user)
    {
        var existingUser = await _userRepository.GetByEmailAsync(user.Email);
        
        if (existingUser != null)
        {
            throw new InvalidOperationException("Ya existe un usuario registrado con este correo electrónico.");
        }

        // En este espacio se encriptara la contraseña mas adelante.

        user.Role = UserRole.Customer;
        var savedUser = await _userRepository.AddAsync(user);
        
        return savedUser;
    }

    public async Task<User> Login(string username, string password)
    {
        var user = await _userRepository.GetByEmailAsync(username);
        
        if (user == null)
        {
            throw new UnauthorizedAccessException("Credenciales incorrectas.");
        }

        if (user.PasswordHash != password) 
        {
            throw new UnauthorizedAccessException("Credenciales incorrectas.");
        }

        return user;
    }
}