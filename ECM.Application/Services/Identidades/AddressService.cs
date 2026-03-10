using ECM.Application.Interfaces.Respository.Identidades;
using ECM.Application.Interfaces.ServicesInterfaces.IdentidadesServices;
using ECM.Domain.Common;
using ECM.Domain.Entities.Identidades;

namespace ECM.Application.Services.Identidades;

public class AddressService : IAddressService
{
    private readonly IAddressRepository _addressRepository;

    public AddressService(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }

    public async Task<OperationResult<Address>> GetByIdAsync(int id)
    {
        if (id <= 0)
        {
            return OperationResult<Address>.Fail("El id no puede ser menor a 0");
        }

        Address? address = await _addressRepository.GetByIdAsync(id);

        if (address == null)
        {
            return OperationResult<Address>.Fail("La direccion no fue encontrada en el repositorio");
        }
        
        return OperationResult<Address>.Ok(address, $"La direccion con el id {id} fue retornada exitosamente");
    }

    public async Task<OperationResult<IEnumerable<Address>>> GetAllAsync()
    {
        var addressList = await _addressRepository.GetAllAsync();

        if (addressList == null)
        {
            return OperationResult<IEnumerable<Address>>.Ok(new List<Address>(), "La lista de direcciones está vacía.");
        }
        
        return OperationResult<IEnumerable<Address>>.Ok(addressList);
    }

    public async Task<OperationResult<Address>> CreateAsync(Address entity)
    {
        if (entity == null)
        {
            return OperationResult<Address>.Fail("La entidad no puede ser nula");
        }
        
        var createdAddress = await _addressRepository.AddAsync(entity);
        
        if (createdAddress == null)
        {
            return OperationResult<Address>.Fail("Error al crear la dirección");
        }
        
        return OperationResult<Address>.Ok(createdAddress, "direccion creada correctamente");
    }

    public async Task<OperationResult<Address>> UpdateAsync(Address entity)
    {
        if (entity == null)
        {
            return OperationResult<Address>.Fail("La direccion no puede ser nula");
        }

        if (entity.Id <= 0)
        {
            return OperationResult<Address>.Fail("El Id no puede ser menor a 0");
        }
        
        var addressToUpdate = await _addressRepository.GetByIdAsync(entity.Id);

        if (addressToUpdate == null)
        {
            return OperationResult<Address>.Fail("La dirección que intentas actualizar no existe.");
        }

        var updatedAddress = await _addressRepository.Update(entity, entity.Id);
        
        if (updatedAddress == null)
        {
            return OperationResult<Address>.Fail("Error al actualizar la dirección");
        }
 
        return OperationResult<Address>.Ok(updatedAddress, "Dirección actualizada correctamente.");
    }

    public async Task<OperationResult> DeleteAsync(int id)
    {
        if (id <= 0)
        {
            return OperationResult.Fail("El id no puede ser menor a 0");
        }
        
        var address = await _addressRepository.GetByIdAsync(id);

        if (address == null)
        {
            return OperationResult.Fail("La direccion no pudo ser encontrada");
        }

        await _addressRepository.Disable(id);

        return OperationResult.Ok("La direccion fue borrada correctamente");
    }
}