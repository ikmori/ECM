using System.Threading.Tasks;
using ECM.Application.Interfaces.BaseRepository;
using ECM.Domain.Entities.Identidades;

namespace ECM.Application.Interfaces.Respository.Identidades
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetUserWithAddressesAsync(int userId);
    }
}