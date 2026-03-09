using ECM.Application.Interfaces.Respository.Identidades;
using ECM.Application.Interfaces.ServicesInterfaces.IdentidadesServices;
using ECM.Application.Services.Identidades;
using ECM.Domain.Entities.Identidades;
using Moq;

namespace ECM.Test.Services.Identidades;

public class AddressServiceTest
{
    private readonly IAddressService _addressService;
    private readonly Mock<IAddressRepository> _addressRepositoryMock;

    public AddressServiceTest()
    {
        _addressRepositoryMock = new Mock<IAddressRepository>();
        _addressService = new AddressService(_addressRepositoryMock.Object);
    }
    
    /// <summary>
    /// GetByIdAsync
    /// </summary>
    [Fact]
    public async Task GetByIdAsync_WhenIdIsZeroOrLess_ShouldReturnFail()
    {
        // Arrange
        int invalidId = 0; // Probamos con 0 (según tu validación <= 0)

        // Act
        var result = await _addressService.GetByIdAsync(invalidId);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("El id no puede ser menor a 0", result.Message);
        
        // Verificamos que no haya ido a consultar la base de datos
        _addressRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetByIdAsync_WhenAddressNotFound_ShouldReturnFail()
    {
        // Arrange
        int validId = 1;
        
        // Simulamos que el repositorio no encuentra la dirección
        _addressRepositoryMock
            .Setup(repo => repo.GetByIdAsync(validId))
            .ReturnsAsync((Address)null);

        // Act
        var result = await _addressService.GetByIdAsync(validId);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("La direccion no fue encontrada en el repositorio", result.Message);
    }

    [Fact]
    public async Task GetByIdAsync_WhenAddressExists_ShouldReturnSuccess()
    {
        // Arrange
        int validId = 1;
        var existingAddress = new Address { Id = validId, Street = "Calle Principal 123" };

        // Simulamos que el repositorio sí la encuentra
        _addressRepositoryMock
            .Setup(repo => repo.GetByIdAsync(validId))
            .ReturnsAsync(existingAddress);

        // Act
        var result = await _addressService.GetByIdAsync(validId);

        // Assert
        Assert.True(result.Success);
        Assert.Equal($"La direccion con el id {validId} fue retornada exitosamente", result.Message);
        Assert.NotNull(result.Data);
        Assert.Equal(validId, result.Data.Id);
    }

    /// <summary>
    /// GetAllAsync
    /// </summary>

    [Fact]
    public async Task GetAllAsync_WhenListIsEmpty_ShouldReturnOkWithEmptyList()
    {
        // Arrange
        // Simulamos que la base de datos devuelve una lista vacía
        _addressRepositoryMock
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(new List<Address>());

        // Act
        var result = await _addressService.GetAllAsync();

        // Assert
        Assert.True(result.Success); 
        Assert.Equal("La lista de direcciones está vacía.", result.Message);
        Assert.NotNull(result.Data);
        Assert.Empty(result.Data);
    }

    [Fact]
    public async Task GetAllAsync_WhenAddressesExist_ShouldReturnSuccessAndList()
    {
        // Arrange
        var addressList = new List<Address> 
        { 
            new Address { Id = 1, Street = "Calle A" },
            new Address { Id = 2, Street = "Avenida B" }
        };

        // Simulamos que el repositorio devuelve datos
        _addressRepositoryMock
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(addressList);

        // Act
        var result = await _addressService.GetAllAsync();

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal(2, result.Data.Count()); // Verificamos que traiga exactamente 2 elementos
    }
    
    /// <summary>
    /// CreateAsync
    /// </summary>

    [Fact]
    public async Task CreateAsync_WhenEntityIsNull_ShouldReturnFail()
    {
        // Arrange
        Address address = null;

        // Act
        var result = await _addressService.CreateAsync(address);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("La entidad no puede ser nula", result.Message);
        
        // Verificamos que no intentó guardar en la base de datos
        _addressRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Address>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WhenDataIsValid_ShouldReturnSuccess()
    {
        // Arrange
        var newAddress = new Address { Street = "Calle Nueva 456", City = "Santo Domingo" };
        var savedAddress = new Address { Id = 1, Street = "Calle Nueva 456", City = "Santo Domingo" };

        // Simulamos que el repositorio guarda y devuelve la entidad con su ID asignado
        _addressRepositoryMock
            .Setup(repo => repo.AddAsync(newAddress))
            .ReturnsAsync(savedAddress);

        // Act
        var result = await _addressService.CreateAsync(newAddress);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("direccion creada correctamente", result.Message); 
        Assert.NotNull(result.Data);
        Assert.Equal(1, result.Data.Id);
        
        // Verificamos que se llamó al método de guardar una sola vez
        _addressRepositoryMock.Verify(repo => repo.AddAsync(newAddress), Times.Once);
    }

    /// <summary>
    /// UpdateAsync
    /// </summary>

    [Fact]
    public async Task UpdateAsync_WhenEntityIsNull_ShouldReturnFail()
    {
        // Arrange
        Address address = null;

        // Act
        var result = await _addressService.UpdateAsync(address);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("La direccion no puede ser nula", result.Message);
    }

    [Fact]
    public async Task UpdateAsync_WhenIdIsZeroOrLess_ShouldReturnFail()
    {
        // Arrange
        var address = new Address { Id = 0, Street = "Calle Inválida" };

        // Act
        var result = await _addressService.UpdateAsync(address);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("El Id no puede ser menor a 0", result.Message);
    }

    [Fact]
    public async Task UpdateAsync_WhenAddressDoesNotExist_ShouldReturnFail()
    {
        // Arrange
        var addressToUpdate = new Address { Id = 99, Street = "Calle Fantasma" };

        // Simulamos que el repositorio busca por ID y devuelve null (no existe)
        _addressRepositoryMock
            .Setup(repo => repo.GetByIdAsync(addressToUpdate.Id))
            .ReturnsAsync((Address)null);

        // Act
        var result = await _addressService.UpdateAsync(addressToUpdate);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("La dirección que intentas actualizar no existe.", result.Message); 
        
        // Verificamos que no intentó actualizar
        _addressRepositoryMock.Verify(repo => repo.Update(It.IsAny<Address>(), It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WhenDataIsValid_ShouldReturnSuccess()
    {
        // Arrange
        var addressToUpdate = new Address { Id = 1, Street = "Calle Corregida 123" };
        var existingAddress = new Address { Id = 1, Street = "Calle Vieja 123" };

        // Simulamos que la dirección existe en la base de datos
        _addressRepositoryMock
            .Setup(repo => repo.GetByIdAsync(addressToUpdate.Id))
            .ReturnsAsync(existingAddress);

        // Simulamos el proceso de actualización
        _addressRepositoryMock
            .Setup(repo => repo.Update(addressToUpdate, addressToUpdate.Id))
            .ReturnsAsync(addressToUpdate);

        // Act
        var result = await _addressService.UpdateAsync(addressToUpdate);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Dirección actualizada correctamente.", result.Message);
        
        // Verificamos que el repositorio ejecutó la actualización
        _addressRepositoryMock.Verify(repo => repo.Update(addressToUpdate, addressToUpdate.Id), Times.Once);
    }
    
    /// <summary>
    /// DeleteAsync
    /// </summary>

    [Fact]
    public async Task DeleteAsync_WhenIdIsZeroOrLess_ShouldReturnFail()
    {
        // Arrange
        int invalidId = 0;

        // Act
        var result = await _addressService.DeleteAsync(invalidId);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("El id no puede ser menor a 0", result.Message);
        
        // Verificamos que el repositorio no se haya llamado para buscar ni para deshabilitar
        _addressRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<int>()), Times.Never);
        _addressRepositoryMock.Verify(repo => repo.Disable(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_WhenAddressDoesNotExist_ShouldReturnFail()
    {
        // Arrange
        int validId = 1;

        // Simulamos que el repositorio no encuentra la dirección
        _addressRepositoryMock
            .Setup(repo => repo.GetByIdAsync(validId))
            .ReturnsAsync((Address)null);

        // Act
        var result = await _addressService.DeleteAsync(validId);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("La direccion no pudo ser encontrada", result.Message);
        
        // Verificamos que no se haya llamado al método Disable
        _addressRepositoryMock.Verify(repo => repo.Disable(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_WhenAddressExists_ShouldReturnSuccess()
    {
        // Arrange
        int validId = 1;
        var existingAddress = new Address { Id = validId, Street = "Calle Principal" };

        // Simulamos que la dirección sí existe
        _addressRepositoryMock
            .Setup(repo => repo.GetByIdAsync(validId))
            .ReturnsAsync(existingAddress);

        // Act
        var result = await _addressService.DeleteAsync(validId);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("La direccion fue borrada correctamente", result.Message);
        
        // Verificamos que el repositorio haya sido llamado para aplicar el borrado lógico (Disable)
        _addressRepositoryMock.Verify(repo => repo.Disable(validId), Times.Once);
    }
}