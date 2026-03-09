using ECM.Application.Interfaces.ServicesInterfaces.Servicio_Base;
using ECM.Domain.Common;
using ECM.Domain.Entities.Identidades;

namespace ECM.Application.Interfaces.ServicesInterfaces.IdentidadesServices;

public interface IUserService: IBaseService<User>
{
    Task<OperationResult<User>> Login(string email, string password);
    Task<OperationResult<User>> Register(User user);
}