using ECM.Application.Interfaces.Respository.Identidades;
using ECM.Application.Interfaces.ServicesInterfaces.IdentidadesServices;
using ECM.Application.Services.Identidades;
using ECM.Domain.Common;
using ECM.Domain.Entities.Identidades;
using Moq;
using Xunit;

namespace ECM.Test.Services.Identidades;

public class UserServiceTest
{
    private readonly IUserService _userService;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    
    public UserServiceTest()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userService = new UserService(_userRepositoryMock.Object);
    }

    /// <summary>
    /// GetByIdAsync
    /// </summary>

    [Fact]
    public async Task GetByIdAsync_WhenIdIsLessThanZero_ShouldReturnFail()
    {
        // Arrange
        int invalidId = -1;

        // Act
        var result = await _userService.GetByIdAsync(invalidId);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("el id no puede ser menor a 0", result.Message);
        // Verificamos que no haya intentado ir a la base de datos
        _userRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetByIdAsync_WhenUserDoesNotExist_ShouldReturnFail()
    {
        // Arrange
        int validId = 1;
        
        // Simulamos que el repositorio no encuentra nada y devuelve null
        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(validId))
            .ReturnsAsync((User)null);

        // Act
        var result = await _userService.GetByIdAsync(validId);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("el usuario no fue encontrado", result.Message);
    }

    [Fact]
    public async Task GetByIdAsync_WhenUserExists_ShouldReturnSuccess()
    {
        // Arrange
        int validId = 1;
        var existingUser = new User { Id = validId, Email = "test@correo.com" };

        // Simulamos que el repositorio encuentra al usuario
        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(validId))
            .ReturnsAsync(existingUser);

        // Act
        var result = await _userService.GetByIdAsync(validId);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("El usuario fue encontrado correctamente", result.Message);
        Assert.NotNull(result.Data);
        Assert.Equal(validId, result.Data.Id);
    }

    /// <summary>
    /// GetAllAsync
    /// </summary>

    [Fact]
    public async Task GetAllAsync_WhenNoUsersExist_ShouldReturnOkWithEmptyList()
    {
        // Arrange
        // Simulamos que el repositorio devuelve una lista vacía
        _userRepositoryMock
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(new List<User>());

        // Act
        var result = await _userService.GetAllAsync();

        // Assert
        Assert.True(result.Success); // Fíjate que tu método devuelve Ok (True) en este caso, lo cual está bien
        Assert.Equal("No hay usuarios registrados actualmente.", result.Message);
        Assert.NotNull(result.Data);
        Assert.Empty(result.Data); // Verificamos que la colección devuelta esté vacía
    }

    [Fact]
    public async Task GetAllAsync_WhenUsersExist_ShouldReturnSuccessAndList()
    {
        // Arrange
        var usersList = new List<User> 
        { 
            new User { Id = 1, Email = "user1@correo.com" },
            new User { Id = 2, Email = "user2@correo.com" }
        };

        // Simulamos que el repositorio devuelve nuestra lista llena
        _userRepositoryMock
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(usersList);

        // Act
        var result = await _userService.GetAllAsync();

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Listado de todos los usuarios devuelto correctamente", result.Message);
        Assert.NotNull(result.Data);
        Assert.Equal(2, result.Data.Count()); // Verificamos que devuelva los 2 usuarios
    }
    
    /// <summary>
    /// CreateAsync
    /// </summary>
    [Fact]
    public async Task CreateAsync_WhenUserIsNull_ShouldReturnFail()
    {
        User user = null;
        
        var result = await _userService.CreateAsync(user);
        
        Assert.False(result.Success);
        Assert.Equal("El usuario no puede ser nulo", result.Message);
    }
    
    [Fact]
    public async Task CreateAsync_WhenEmailIsEmpty_ShouldReturnFail()
    {
        var userWithoutEmail = new User { Email = "" };

        var result = await _userService.CreateAsync(userWithoutEmail);

        Assert.False(result.Success);
        Assert.Equal("El email no puede estar vacio", result.Message);
    }

    [Fact]
    public async Task CreateAsync_WhenEmailAlreadyExists_ShouldReturnFail()
    {
        var newUser = new User { Email = "test@correo.com" };
        var existingUser = new User { Email = "test@correo.com", Id = 1 };

        _userRepositoryMock
            .Setup(repo => repo.GetByEmailAsync(newUser.Email))
            .ReturnsAsync(existingUser);

        var result = await _userService.CreateAsync(newUser);

        Assert.False(result.Success);
        Assert.Equal("El email ingresado ya existe", result.Message);
        
        _userRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WhenDataIsValid_ShouldReturnSuccess()
    {
        var newUser = new User { Email = "nuevo@correo.com", FirstName = "Juan" };

        _userRepositoryMock
            .Setup(repo => repo.GetByEmailAsync(newUser.Email))
            .ReturnsAsync((User)null);

        var savedUser = new User { Id = 1, Email = "nuevo@correo.com", FirstName = "Juan" };
        
        _userRepositoryMock
            .Setup(repo => repo.AddAsync(newUser))
            .ReturnsAsync(savedUser);

        var result = await _userService.CreateAsync(newUser);

        Assert.True(result.Success);
        Assert.Equal("Usuario agregado correctamente", result.Message);
        Assert.NotNull(result.Data);
        Assert.Equal(1, result.Data.Id); 
        
        _userRepositoryMock.Verify(repo => repo.AddAsync(newUser), Times.Once);
    }
    
    /// <summary>
    /// UpdateAsync
    /// </summary>
    [Fact]
    public async Task UpdateAsync_WhenUserIsNull_ShouldReturnFail()
    {
        User user = null;

        var result = await _userService.UpdateAsync(user);

        Assert.False(result.Success);
        Assert.Equal("El Usuario no puede ser nulo", result.Message);
    }

    [Fact]
    public async Task UpdateAsync_WhenIdIsZeroOrLess_ShouldReturnFail()
    {
        var user = new User { Id = 0, Email = "test@correo.com" };

        var result = await _userService.UpdateAsync(user);

        Assert.False(result.Success);
        Assert.Equal("El Id no puede ser menor a 0", result.Message);
    }

    [Fact]
    public async Task UpdateAsync_WhenEmailIsBlank_ShouldReturnFail()
    {
        var user = new User { Id = 1, Email = "   " };

        var result = await _userService.UpdateAsync(user);

        Assert.False(result.Success);
        Assert.Equal("El email ingresado no puede estar en blanco", result.Message);
    }

    [Fact]
    public async Task UpdateAsync_WhenEmailBelongsToAnotherUser_ShouldReturnFail()
    {
        var userToUpdate = new User { Id = 1, Email = "ocupado@correo.com" };
        
        // Simulamos que el correo ya está registrado en la BD bajo el ID 2 (otro usuario diferente)
        var anotherUser = new User { Id = 2, Email = "ocupado@correo.com" };

        _userRepositoryMock
            .Setup(repo => repo.GetByEmailAsync(userToUpdate.Email))
            .ReturnsAsync(anotherUser);

        var result = await _userService.UpdateAsync(userToUpdate);

        Assert.False(result.Success);
        Assert.Equal("El correo electrónico que intentas usar ya le pertenece a otro usuario.", result.Message);
        
        _userRepositoryMock.Verify(repo => repo.Update(It.IsAny<User>(), It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WhenDataIsValid_ShouldReturnSuccess()
    {
        var userToUpdate = new User { Id = 1, Email = "mi@correo.com", FirstName = "Pedro" };
        
        // Simulamos que el correo existe, pero pertenece al MISMO usuario (Id = 1)
        var existingUser = new User { Id = 1, Email = "mi viejocorreo@correo.com" }; 

        _userRepositoryMock
            .Setup(repo => repo.GetByEmailAsync(userToUpdate.Email))
            .ReturnsAsync(existingUser);

        _userRepositoryMock
            .Setup(repo => repo.Update(userToUpdate, userToUpdate.Id))
            .ReturnsAsync(userToUpdate);

        var result = await _userService.UpdateAsync(userToUpdate);

        Assert.True(result.Success);
        Assert.Equal("Usuario actualizado correctamente", result.Message);
        
        _userRepositoryMock.Verify(repo => repo.Update(userToUpdate, userToUpdate.Id), Times.Once);
    }

    /// <summary>
    /// DeleteAsync
    /// </summary>

    [Fact]
    public async Task DeleteAsync_WhenUserDoesNotExist_ShouldReturnFail()
    {
        int validId = 1;

        // Simulamos que el usuario no se encuentra en la base de datos
        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(validId))
            .ReturnsAsync((User)null);

        var result = await _userService.DeleteAsync(validId);

        Assert.False(result.Success);
        Assert.Equal("El usuario no existe", result.Message);
        
        // Nos aseguramos de que no haya intentado deshabilitarlo
        _userRepositoryMock.Verify(repo => repo.Disable(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_WhenUserExists_ShouldReturnSuccess()
    {
        int validId = 1;
        var existingUser = new User { Id = validId };

        // Simulamos que el usuario sí existe
        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(validId))
            .ReturnsAsync(existingUser);

        var result = await _userService.DeleteAsync(validId);

        Assert.True(result.Success);
        Assert.Equal("Usuario eliminado correctamente", result.Message);
        
        // Verificamos que el repositorio haya sido llamado para deshabilitar el usuario
        _userRepositoryMock.Verify(repo => repo.Disable(validId), Times.Once);
    }
}