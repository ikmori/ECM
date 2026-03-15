using ECM.Domain.Common.Enums;
using ECM.Domain.Entities.Logistica;

namespace ECM.Application.Services.Logistica;

public class CouponService
{
    private readonly List<Coupon> _coupons; // Simulando repositorio

    public CouponService()
    {
        _coupons = new List<Coupon>
        {
            new Coupon
            {
                Id = 1,
                Code = "WELCOME10",
                Type = DiscountType.Percentage,
                Value = 10,
                MinimumOrderAmount = 100,
                ExpirationDate = DateTime.UtcNow.AddMonths(1),
                MaxUsage = 100,
                TimesUsed = 0
            },
            new Coupon
            {
                Id = 2,
                Code = "SAVE20",
                Type = DiscountType.FixedAmount,
                Value = 20,
                MinimumOrderAmount = 50,
                ExpirationDate = DateTime.UtcNow.AddMonths(1),
                MaxUsage = 50,
                TimesUsed = 0
            }
        };
    }

    public Coupon? ValidateCoupon(string code, decimal orderAmount)
    {
        var coupon = _coupons.FirstOrDefault(c => c.Code == code);
        
        if (coupon == null)
            return null;

        if (coupon.ExpirationDate < DateTime.UtcNow)
            return null;

        if (coupon.TimesUsed >= coupon.MaxUsage)
            return null;

        if (coupon.MinimumOrderAmount.HasValue && orderAmount < coupon.MinimumOrderAmount)
            return null;

        return coupon;
    }

    public decimal CalculateDiscount(Coupon coupon, decimal orderAmount)
    {
        return coupon.Type switch
        {
            DiscountType.Percentage => orderAmount * (coupon.Value / 100),
            DiscountType.FixedAmount => coupon.Value,
            _ => 0
        };
    }

    public void UseCoupon(int couponId)
    {
        var coupon = _coupons.FirstOrDefault(c => c.Id == couponId);
        if (coupon != null)
        {
            coupon.TimesUsed++;
        }
    }
}