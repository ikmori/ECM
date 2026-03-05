using ECM.Application.Interfaces.Respository.Identidades;
using ECM.Application.Interfaces.ServicesInterfaces.IdentidadesServices;
using ECM.Domain.Entities.Identidades;

namespace ECM.Application.Services.Identidades;

public class AddressService : IAddressService
{
    private readonly IAddressRepository _addressRepository;

    public AddressService(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }

    public async Task<Address?> GetByIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("El ID de la dirección debe ser mayor a cero.", nameof(id));
        }

        return await _addressRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Address>> GetAllAsync()
    {
        return await _addressRepository.GetAllAsync();
    }

    public async Task CreateAsync(Address entity)
    {
        ValidateAddressEntity(entity);

        await _addressRepository.AddAsync(entity);
    }

    public async Task UpdateAsync(Address entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "La dirección no puede ser nula.");
        }

        if (entity.Id <= 0)
        {
            throw new ArgumentException("El ID de la dirección a actualizar no es válido.", nameof(entity.Id));
        }

        ValidateAddressEntity(entity);

        await _addressRepository.Update(entity, entity.Id);
    }

    public async Task DeleteAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("El ID de la dirección debe ser mayor a cero.", nameof(id));
        }

        await _addressRepository.Disable(id);
    }

    // --- MÉTODOS AUXILIARES ---

    /// <summary>
    /// Centraliza las validaciones de negocio para una dirección.
    /// </summary>
    private void ValidateAddressEntity(Address entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "La dirección no puede ser nula.");
        }
        
        if (string.IsNullOrWhiteSpace(entity.Street))
        {
            throw new ArgumentException("La calle (Street) es obligatoria.", nameof(entity.Street));
        }

        if (string.IsNullOrWhiteSpace(entity.City))
        {
            throw new ArgumentException("La ciudad (City) es obligatoria.", nameof(entity.City));
        }
        
    }
}