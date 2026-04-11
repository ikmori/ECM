using Microsoft.EntityFrameworkCore;
using ECM.Application.Interfaces.Respository.Logistica;
using ECM.Data.Context;
using ECM.Domain.Entities.Logistica;
using ECM.Domain.Common.Enums;

namespace ECM.Data.Repositories.Logistica
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _context;

        public ReviewRepository(AppDbContext context)
        {
            _context = context;
        }

        // Métodos base de IBaseRepository
        public async Task<Review?> GetByIdAsync(int id)
        {
            return await _context.Reviews
                .Include(r => r.Product)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id && r.IsActive);
        }

        public async Task<IEnumerable<Review>> GetAllAsync()
        {
            return await _context.Reviews
                .Where(r => r.IsActive)
                .Include(r => r.Product)
                .Include(r => r.User)
                .ToListAsync();
        }

        public async Task<Review?> AddAsync(Review entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            entity.IsActive = true;
            
            await _context.Reviews.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Review?> Update(Review entity, int id)
        {
            var existingReview = await _context.Reviews.FindAsync(id);
            if (existingReview == null) return null;

            entity.Id = id;
            entity.CreatedAt = existingReview.CreatedAt;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.IsActive = existingReview.IsActive;
            
            _context.Entry(existingReview).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            
            return existingReview;
        }

        public async Task<Review?> Disable(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review != null)
            {
                review.IsActive = false;
                review.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
            return review;
        }

        // Métodos específicos de IReviewRepository
        public async Task<IEnumerable<Review>> GetByProductIdAsync(int productId)
        {
            return await _context.Reviews
                .Where(r => r.ProductId == productId && r.IsActive)
                .Include(r => r.User)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetByUserIdAsync(int userId)
        {
            return await _context.Reviews
                .Where(r => r.UserId == userId && r.IsActive)
                .Include(r => r.Product)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<double> GetAverageRatingAsync(int productId)
        {
            var average = await _context.Reviews
                .Where(r => r.ProductId == productId && r.IsActive)
                .AverageAsync(r => r.Rating);
            
            return Math.Round(average, 1);
        }

        public async Task<bool> HasUserReviewedProductAsync(int userId, int productId)
        {
            return await _context.Reviews
                .AnyAsync(r => r.UserId == userId && 
                              r.ProductId == productId && 
                              r.IsActive);
        }

        public async Task<bool> CanUserReviewProductAsync(int userId, int productId)
        {
            // Verificar si el usuario compró el producto y fue entregado
            var hasPurchased = await _context.Orders
                .AnyAsync(o => o.UserId == userId && 
                              o.Status == OrderStatus.Delivered &&
                              o.OrderItems.Any(oi => oi.ProductId == productId));
            
            if (!hasPurchased) return false;
            
            // Verificar si ya reseñó
            var hasReviewed = await HasUserReviewedProductAsync(userId, productId);
            
            return !hasReviewed;
        }
        
    }
}