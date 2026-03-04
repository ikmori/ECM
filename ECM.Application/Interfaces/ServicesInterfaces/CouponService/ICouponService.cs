using System.Threading.Tasks;

namespace ECM.Application.Interfaces.ServicesInterfaces.CouponService;

public interface ICouponService
{
    public interface ICouponService
    {
        Task<bool> ApplyCouponAsync(string code, int orderId);
        Task DisableExpiredCouponsAsync();
    }
}