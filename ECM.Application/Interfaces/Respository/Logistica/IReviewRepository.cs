using ECM.Application.Interfaces.BaseRepository;
using ECM.Domain.Entities.Logistica;

namespace ECM.Application.Interfaces.Respository.Logistica;

public interface IReviewRepository : IBaseRepository<Review>
{
    Task<IEnumerable<Review>> GetByProductIdAsync(int productId);
    Task<IEnumerable<Review>> GetByUserIdAsync(int userId);
    Task<double> GetAverageRatingAsync(int productId);
    Task<bool> HasUserReviewedProductAsync(int userId, int productId);
    Task<bool> CanUserReviewProductAsync(int userId, int productId);
    // El método Update viene de IBaseRepository<Review>
}