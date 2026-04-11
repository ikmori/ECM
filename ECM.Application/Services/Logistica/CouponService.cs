using ECM.Application.Interfaces.Respository.Logistica;
using ECM.Domain.Common;
using ECM.Domain.Common.Enums;
using ECM.Domain.Entities.Logistica;


namespace ECM.Application.Services.Logistica;

public class CouponService
{
    private readonly ICouponRepository _couponRepository;

    public CouponService(ICouponRepository couponRepository)
    {
        _couponRepository = couponRepository;
    }

    public CouponService()
    {
        throw new NotImplementedException();
    }

    public async Task<OperationResult<Coupon>> GetCouponByIdAsync(int id)
    {
        try
        {
            var coupon = await _couponRepository.GetByIdAsync(id);
            if (coupon == null)
                return OperationResult<Coupon>.Fail($"Cupón con ID {id} no encontrado.");
            
            return OperationResult<Coupon>.Ok(coupon, "Cupón recuperado exitosamente.");
        }
        catch (Exception ex)
        {
            return OperationResult<Coupon>.Fail($"Error al recuperar cupón: {ex.Message}");
        }
    }

  
    public async Task<OperationResult<IEnumerable<Coupon>>> GetValidCouponsAsync(decimal? orderAmount = null)
    {
        try
        {
            var coupons = await _couponRepository.GetValidCouponsAsync(orderAmount);
            return OperationResult<IEnumerable<Coupon>>.Ok(coupons, "Cupones recuperados exitosamente.");
        }
        catch (Exception ex)
        {
            return OperationResult<IEnumerable<Coupon>>.Fail($"Error al recuperar cupones: {ex.Message}");
        }
    }
    
    public async Task<OperationResult<Coupon?>> ValidateCouponAsync(string code, decimal orderAmount)
    {
        try
        {
            var isValid = await _couponRepository.IsValidCouponAsync(code, orderAmount);
            if (!isValid)
                return OperationResult<Coupon?>.Fail("El cupón no es válido o no cumple las condiciones.");

            var coupon = await _couponRepository.GetByCodeAsync(code);
            return OperationResult<Coupon?>.Ok(coupon, "Cupón válido.");
        }
        catch (Exception ex)
        {
            return OperationResult<Coupon?>.Fail($"Error al validar cupón: {ex.Message}");
        }
    }
    
    public async Task<OperationResult<Coupon>> CreateCouponAsync(string code, DiscountType type, decimal value, 
        DateTime expirationDate, int maxUsage, decimal? minimumOrderAmount = null)
    {
        try
        {
            // Verificar si ya existe un cupón con ese código
            var existing = await _couponRepository.GetByCodeAsync(code);
            if (existing != null)
                return OperationResult<Coupon>.Fail($"Ya existe un cupón con el código {code}.");

            var coupon = new Coupon
            {
                Code = code.ToUpper(),
                Type = type,
                Value = value,
                ExpirationDate = expirationDate,
                MaxUsage = maxUsage,
                MinimumOrderAmount = minimumOrderAmount,
                TimesUsed = 0,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            var created = await _couponRepository.AddAsync(coupon);
            if (created == null)
                return OperationResult<Coupon>.Fail("Error al crear el cupón.");
            
            return OperationResult<Coupon>.Ok(created, "Cupón creado exitosamente.");
        }
        catch (Exception ex)
        {
            return OperationResult<Coupon>.Fail($"Error al crear cupón: {ex.Message}");
        }
    }

    public async Task<OperationResult<Coupon>> UpdateCouponAsync(int id, decimal value, decimal? minimumOrderAmount, DateTime expirationDate, int maxUsage)
    {
        try
        {
            var existing = await _couponRepository.GetByIdAsync(id);
            if (existing == null)
                return OperationResult<Coupon>.Fail($"Cupón con ID {id} no encontrado.");
        
            // Actualizar solo los campos permitidos
            existing.Value = value;
            existing.MinimumOrderAmount = minimumOrderAmount;
            existing.ExpirationDate = expirationDate;
            existing.MaxUsage = maxUsage;
            existing.UpdatedAt = DateTime.UtcNow;
        
            var updated = await _couponRepository.Update(existing, id);
            if (updated == null)
                return OperationResult<Coupon>.Fail("Error al actualizar el cupón.");
        
            return OperationResult<Coupon>.Ok(updated, "Cupón actualizado exitosamente.");
        }
        catch (Exception ex)
        {
            return OperationResult<Coupon>.Fail($"Error al actualizar cupón: {ex.Message}");
        }
    }
    
    public async Task<OperationResult<bool>> DisableCouponAsync(int couponId)
    {
        try
        {
            var coupon = await _couponRepository.Disable(couponId);
            if (coupon == null)
                return OperationResult<bool>.Fail($"Cupón con ID {couponId} no encontrado.");
            
            return OperationResult<bool>.Ok(true, "Cupón deshabilitado exitosamente.");
        }
        catch (Exception ex)
        {
            return OperationResult<bool>.Fail($"Error al deshabilitar cupón: {ex.Message}");
        }
    }

    public async Task<OperationResult<decimal>> CalculateDiscountAsync(string couponCode, decimal orderAmount)
    {
        try
        {
            var coupon = await _couponRepository.GetByCodeAsync(couponCode);
            if (coupon == null)
                return OperationResult<decimal>.Fail("Cupón no encontrado.");

            var discount = coupon.Type switch
            {
                DiscountType.Percentage => orderAmount * (coupon.Value / 100),
                DiscountType.FixedAmount => coupon.Value,
                _ => 0
            };

            return OperationResult<decimal>.Ok(discount, "Descuento calculado.");
        }
        catch (Exception ex)
        {
            return OperationResult<decimal>.Fail($"Error al calcular descuento: {ex.Message}");
        }
    }
    
    public async Task<OperationResult<bool>> ApplyCouponToOrderAsync(string couponCode, int orderId)
    {
        try
        {
            var coupon = await _couponRepository.GetByCodeAsync(couponCode);
            if (coupon == null)
                return OperationResult<bool>.Fail("Cupón no encontrado.");

            var used = await _couponRepository.IncrementTimesUsedAsync(coupon.Id);
            if (!used)
                return OperationResult<bool>.Fail("Error al aplicar el cupón.");

            return OperationResult<bool>.Ok(true, "Cupón aplicado exitosamente.");
        }
        catch (Exception ex)
        {
            return OperationResult<bool>.Fail($"Error al aplicar cupón: {ex.Message}");
        }
    }

    public object? ValidateCoupon(string couponCode, decimal orderAmount)
    {
        throw new NotImplementedException();
    }
}

