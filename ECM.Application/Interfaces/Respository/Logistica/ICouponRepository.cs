using Domain.Interfaces.Repositories;
using ECM.Domain.Entities.Logistica;

namespace ECM.Application.Interfaces.Respository.Marketing;

public interface ICouponRepository
{
    public interface ICouponRepository : IBaseRepository<Coupon>
    {
        Task<Coupon?> GetByCodeAsync(string code);
        Task<bool> IsValidCouponAsync(string code, decimal orderAmount);
    }
}