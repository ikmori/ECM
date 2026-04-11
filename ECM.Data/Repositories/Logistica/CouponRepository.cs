using Microsoft.EntityFrameworkCore;
using ECM.Application.Interfaces.Respository.Logistica;
using ECM.Data.Context;
using ECM.Domain.Entities.Logistica;
using ECM.Domain.Common.Enums;

namespace ECM.Data.Repositories.Logistica
{
    public class CouponRepository : ICouponRepository
    {
        private readonly AppDbContext _context;

        public CouponRepository(AppDbContext context)
        {
            _context = context;
        }

        // Métodos base de IBaseRepository
        public async Task<Coupon?> GetByIdAsync(int id)
        {
            return await _context.Coupons
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
        }

        public async Task<IEnumerable<Coupon>> GetAllAsync()
        {
            return await _context.Coupons
                .Where(c => c.IsActive)
                .ToListAsync();
        }

        public async Task<Coupon?> AddAsync(Coupon entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            entity.IsActive = true;
            
            await _context.Coupons.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Coupon?> Update(Coupon entity, int id)
        {
            var existingCoupon = await _context.Coupons.FindAsync(id);
            if (existingCoupon == null) return null;

            // Preservar propiedades que no deben cambiar
            entity.Id = id;
            entity.CreatedAt = existingCoupon.CreatedAt;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.IsActive = existingCoupon.IsActive;
            
            _context.Entry(existingCoupon).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            
            return existingCoupon;
        }

        public async Task<Coupon?> Disable(int id)
        {
            var coupon = await _context.Coupons.FindAsync(id);
            if (coupon != null)
            {
                coupon.IsActive = false;
                coupon.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
            return coupon;
        }

        // Métodos específicos de ICouponRepository
        public async Task<Coupon?> GetByCodeAsync(string code)
        {
            return await _context.Coupons
                .FirstOrDefaultAsync(c => c.Code == code && c.IsActive);
        }

        public async Task<IEnumerable<Coupon>> GetValidCouponsAsync(decimal? orderAmount = null)
        {
            var now = DateTime.UtcNow;
            var query = _context.Coupons
                .Where(c => c.IsActive &&
                           c.ExpirationDate > now &&
                           c.TimesUsed < c.MaxUsage);

            if (orderAmount.HasValue)
            {
                query = query.Where(c => !c.MinimumOrderAmount.HasValue || 
                                        c.MinimumOrderAmount <= orderAmount.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<bool> IsValidCouponAsync(string code, decimal orderAmount)
        {
            var coupon = await GetByCodeAsync(code);
            if (coupon == null) return false;
            
            if (coupon.ExpirationDate < DateTime.UtcNow) return false;
            if (coupon.TimesUsed >= coupon.MaxUsage) return false;
            if (coupon.MinimumOrderAmount.HasValue && orderAmount < coupon.MinimumOrderAmount) return false;
            
            return true;
        }

        public async Task<bool> IncrementTimesUsedAsync(int couponId)
        {
            var coupon = await _context.Coupons.FindAsync(couponId);
            if (coupon == null) return false;
            
            coupon.TimesUsed++;
            coupon.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}