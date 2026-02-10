
using Domain.Interfaces.Repositories;
using ECM.Domain.Entities.Identidades;

namespace ECM.Application.Interfaces.Repository.Identidades
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetUserWithAddressesAsync(int userId);
    }
}