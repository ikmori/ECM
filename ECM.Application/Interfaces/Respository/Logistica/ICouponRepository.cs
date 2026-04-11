using ECM.Application.Interfaces.BaseRepository;
using ECM.Domain.Entities.Logistica;

namespace ECM.Application.Interfaces.Respository.Logistica;

public interface ICouponRepository : IBaseRepository<Coupon>
{
    Task<Coupon?> GetByCodeAsync(string code);
    Task<IEnumerable<Coupon>> GetValidCouponsAsync(decimal? orderAmount = null);
    Task<bool> IsValidCouponAsync(string code, decimal orderAmount);
    Task<bool> IncrementTimesUsedAsync(int couponId);
}