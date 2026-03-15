using ECM.Application.Services.Logistica;
using ECM.Domain.Common.Enums;
using ECM.Domain.Entities.Logistica;

namespace ECM.Test.Services.Logistica;

public class CouponServiceTests
{
    private readonly CouponService _couponService;

    public CouponServiceTests()
    {
        _couponService = new CouponService();
    }

    [Fact]
    public void ValidateCoupon_CodigoValido_RetornaCoupon()
    {
        // Arrange
        var couponCode = "WELCOME10";
        var orderAmount = 200m;

        // Act
        var result = _couponService.ValidateCoupon(couponCode, orderAmount);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(couponCode, result.Code);
        Assert.Equal(10, result.Value);
    }

    [Fact]
    public void ValidateCoupon_CodigoInvalido_RetornaNull()
    {
        // Arrange
        var couponCode = "CODIGOINEXISTENTE";
        var orderAmount = 200m;

        // Act
        var result = _couponService.ValidateCoupon(couponCode, orderAmount);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void CalculateDiscount_CuponPorcentaje_RetornaDescuentoCorrecto()
    {
        // Arrange
        var coupon = new Coupon
        {
            Type = DiscountType.Percentage,
            Value = 10
        };
        var orderAmount = 200m;

        // Act
        var discount = _couponService.CalculateDiscount(coupon, orderAmount);

        // Assert
        Assert.Equal(20m, discount);
    }

    [Fact]
    public void CalculateDiscount_CuponFijo_RetornaDescuentoCorrecto()
    {
        // Arrange
        var coupon = new Coupon
        {
            Type = DiscountType.FixedAmount,
            Value = 30
        };
        var orderAmount = 200m;

        // Act
        var discount = _couponService.CalculateDiscount(coupon, orderAmount);

        // Assert
        Assert.Equal(30m, discount);
    }

    [Fact]
    public void UseCoupon_IncrementaVecesUsado()
    {
        // Arrange
        var couponId = 1;
        var couponCode = "WELCOME10";
        var orderAmount = 200m;
        
        var coupon = _couponService.ValidateCoupon(couponCode, orderAmount);
        Assert.NotNull(coupon);
        var usosIniciales = coupon.TimesUsed;

        // Act
        _couponService.UseCoupon(couponId);
        
        // Assert
        var couponActualizado = _couponService.ValidateCoupon(couponCode, orderAmount);
        Assert.NotNull(couponActualizado);
        Assert.Equal(usosIniciales + 1, couponActualizado.TimesUsed);
    }
}