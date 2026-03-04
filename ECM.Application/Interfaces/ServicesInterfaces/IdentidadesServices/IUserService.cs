using ECM.Application.Interfaces.ServicesInterfaces.Servicio_Base;
using ECM.Domain.Entities.Identidades;

namespace ECM.Application.Interfaces.ServicesInterfaces.IdentidadesServices;

public interface IUserService: IBaseService<User>
{
    Task<User> Login(string username, string password);
    Task<User> Register(User user);
}