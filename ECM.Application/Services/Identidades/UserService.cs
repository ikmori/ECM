using ECM.Application.Interfaces.Respository.Identidades;
using ECM.Application.Interfaces.ServicesInterfaces.IdentidadesServices;
using ECM.Domain.Common.Enums;
using ECM.Domain.Entities.Identidades;
using ECM.Domain.Common;
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

    public async Task<OperationResult<User>> GetByIdAsync(int id)
    {
        if (id < 0)
        {
            return OperationResult<User>.Fail("el id no puede ser menor a 0");
        }
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
        {
            return OperationResult<User>.Fail("el usuario no fue encontrado");
        }
        
        return OperationResult<User>.Ok(user, "El usuario fue encontrado correctamente");
    }

    public async Task<OperationResult<IEnumerable<User>>> GetAllAsync()
    {
        var usersList =  await _userRepository.GetAllAsync();

        if (usersList == null || !usersList.Any())
        {
            return OperationResult<IEnumerable<User>>.Ok(new List<User>(), "No hay usuarios registrados actualmente.");
        }
        
        return OperationResult<IEnumerable<User>>.Ok(usersList, "Listado de todos los usuarios devuelto correctamente");
    }

    public async Task<OperationResult<User>> CreateAsync(User entity)
    {
        if (entity == null)
        {
            return OperationResult<User>.Fail("El usuario no puede ser nulo");
        }
        
        if (string.IsNullOrWhiteSpace(entity.Email))
        {
            return OperationResult<User>.Fail("El email no puede estar vacio");
        }

        // 3. Validar que el correo no exista ya en la base de datos
        var existingUser = await _userRepository.GetByEmailAsync(entity.Email);
        if (existingUser != null)
        {
            return OperationResult<User>.Fail("El email ingresado ya existe");
        }
        
        var user = await _userRepository.AddAsync(entity);
        
        return OperationResult<User>.Ok(user, "Usuario agregado correctamente");
    }

    public async Task<OperationResult<User>> UpdateAsync(User entity)
    {
        if (entity == null)
        {
            return OperationResult<User>.Fail("El Usuario no puede ser nulo");
        }

        if (entity.Id <= 0)
        {
            return OperationResult<User>.Fail("El Id no puede ser menor a 0");
        }

        if (string.IsNullOrWhiteSpace(entity.Email))
        {
            return OperationResult<User>.Fail("El email ingresado no puede estar en blanco");
        }

        var existingUserWithEmail = await _userRepository.GetByEmailAsync(entity.Email);

        if (existingUserWithEmail != null && existingUserWithEmail.Id != entity.Id)
        {
            return OperationResult<User>.Fail("El correo electrónico que intentas usar ya le pertenece a otro usuario.");
        }

        var updatedUser = await _userRepository.Update(entity, entity.Id);
        
        return OperationResult<User>.Ok(updatedUser, "Usuario actualizado correctamente");
    }

    public async Task<OperationResult> DeleteAsync(int id)
    {
        var userToDelete = await _userRepository.GetByIdAsync(id);
        if (userToDelete == null)
        {
            return OperationResult.Fail("El usuario no existe");
        }
        
        await _userRepository.Disable(id);

        return OperationResult.Ok("Usuario eliminado correctamente");
    }

    // --- MÉTODOS DE REGLAS DE NEGOCIO ---

    public async Task<OperationResult<User>> Register(User user)
    {
        if (user == null)
        {
            return OperationResult<User>.Fail("Los datos del usuario no pueden ser nulos");
        }

        if (string.IsNullOrWhiteSpace(user.Email))
        {
            return OperationResult<User>.Fail("El email no puede estar vacío");
        }

        var existingUser = await _userRepository.GetByEmailAsync(user.Email);
        
        if (existingUser != null)
        {
            return OperationResult<User>.Fail("El email ingresado ya está registrado, intente con otro correo");
        }

        user.Role = UserRole.Customer;
        var savedUser = await _userRepository.AddAsync(user);
        
        return OperationResult<User>.Ok(savedUser, "Usuario registrado correctamente");
    }

    public async Task<OperationResult<User>> Login(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            return OperationResult<User>.Fail("El correo y la contraseña son obligatorios");
        }

        var user = await _userRepository.GetByEmailAsync(email);
        
        if (user == null)
        {
            return OperationResult<User>.Fail("Correo electrónico no encontrado");
        }

        // Aquí luego usarás algo como BCrypt.Verify(password, user.PasswordHash)
        if (user.PasswordHash != password) 
        {
            return OperationResult<User>.Fail("Contraseña incorrecta");
        }

        return OperationResult<User>.Ok(user, "Usuario logueado correctamente");
    }
}