// ==========================================
// ECM.Application.Services.Logistica.ReviewService.cs
// ==========================================
using ECM.Application.Interfaces.Respository.Logistica;
using ECM.Domain.Common;
using ECM.Domain.Entities.Logistica;

namespace ECM.Application.Services.Logistica;

public class ReviewService
{
    private readonly IReviewRepository _reviewRepository;

    public ReviewService(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<OperationResult<IEnumerable<Review>>> GetAllReviewsAsync()
    {
        try
        {
            var reviews = await _reviewRepository.GetAllAsync();
            return OperationResult<IEnumerable<Review>>.Ok(reviews, "Reseñas recuperadas exitosamente.");
        }
        catch (Exception ex)
        {
            return OperationResult<IEnumerable<Review>>.Fail($"Error al recuperar reseñas: {ex.Message}");
        }
    }

    public async Task<OperationResult<Review>> GetReviewByIdAsync(int id)
    {
        try
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            if (review == null)
                return OperationResult<Review>.Fail($"Reseña con ID {id} no encontrada.");
            
            return OperationResult<Review>.Ok(review, "Reseña recuperada exitosamente.");
        }
        catch (Exception ex)
        {
            return OperationResult<Review>.Fail($"Error al recuperar reseña: {ex.Message}");
        }
    }

    public async Task<OperationResult<IEnumerable<Review>>> GetReviewsByProductIdAsync(int productId)
    {
        try
        {
            var reviews = await _reviewRepository.GetByProductIdAsync(productId);
            return OperationResult<IEnumerable<Review>>.Ok(reviews, $"Reseñas del producto {productId} recuperadas.");
        }
        catch (Exception ex)
        {
            return OperationResult<IEnumerable<Review>>.Fail($"Error al recuperar reseñas: {ex.Message}");
        }
    }

    public async Task<OperationResult<IEnumerable<Review>>> GetReviewsByUserIdAsync(int userId)
    {
        try
        {
            var reviews = await _reviewRepository.GetByUserIdAsync(userId);
            return OperationResult<IEnumerable<Review>>.Ok(reviews, $"Reseñas del usuario {userId} recuperadas.");
        }
        catch (Exception ex)
        {
            return OperationResult<IEnumerable<Review>>.Fail($"Error al recuperar reseñas: {ex.Message}");
        }
    }

    public async Task<OperationResult<double>> GetAverageRatingAsync(int productId)
    {
        try
        {
            var average = await _reviewRepository.GetAverageRatingAsync(productId);
            return OperationResult<double>.Ok(average, $"Rating promedio del producto {productId}.");
        }
        catch (Exception ex)
        {
            return OperationResult<double>.Fail($"Error al calcular rating promedio: {ex.Message}");
        }
    }

    public async Task<OperationResult<bool>> CanUserReviewProductAsync(int userId, int productId)
    {
        try
        {
            var canReview = await _reviewRepository.CanUserReviewProductAsync(userId, productId);
            if (!canReview)
                return OperationResult<bool>.Fail("El usuario no puede reseñar este producto. Debe comprarlo primero o ya lo reseñó.");
            
            return OperationResult<bool>.Ok(true, "El usuario puede reseñar este producto.");
        }
        catch (Exception ex)
        {
            return OperationResult<bool>.Fail($"Error al verificar: {ex.Message}");
        }
    }

    public async Task<OperationResult<Review>> CreateReviewAsync(int productId, int userId, int rating, string? comment)
    {
        try
        {
            // Validar rating
            if (rating < 1 || rating > 5)
                return OperationResult<Review>.Fail("El rating debe estar entre 1 y 5.");
            
            // Verificar si puede reseñar
            var canReview = await _reviewRepository.CanUserReviewProductAsync(userId, productId);
            if (!canReview)
                return OperationResult<Review>.Fail("No puedes reseñar este producto. Debes comprarlo primero o ya lo reseñaste.");
            
            var review = new Review
            {
                ProductId = productId,
                UserId = userId,
                Rating = rating,
                Comment = comment,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
            
            var created = await _reviewRepository.AddAsync(review);
            if (created == null)
                return OperationResult<Review>.Fail("Error al crear la reseña.");
            
            return OperationResult<Review>.Ok(created, "Reseña creada exitosamente.");
        }
        catch (Exception ex)
        {
            return OperationResult<Review>.Fail($"Error al crear reseña: {ex.Message}");
        }
    }

    public async Task<OperationResult<Review>> UpdateReviewAsync(int reviewId, int rating, string? comment)
    {
        try
        {
            if (rating < 1 || rating > 5)
                return OperationResult<Review>.Fail("El rating debe estar entre 1 y 5.");
            
            var existing = await _reviewRepository.GetByIdAsync(reviewId);
            if (existing == null)
                return OperationResult<Review>.Fail($"Reseña con ID {reviewId} no encontrada.");
            
            existing.Rating = rating;
            existing.Comment = comment;
            existing.UpdatedAt = DateTime.UtcNow;
            
            var updated = await _reviewRepository.Update(existing, reviewId);
            if (updated == null)
                return OperationResult<Review>.Fail("Error al actualizar la reseña.");
            
            return OperationResult<Review>.Ok(updated, "Reseña actualizada exitosamente.");
        }
        catch (Exception ex)
        {
            return OperationResult<Review>.Fail($"Error al actualizar reseña: {ex.Message}");
        }
    }

    public async Task<OperationResult<bool>> DeleteReviewAsync(int reviewId)
    {
        try
        {
            var review = await _reviewRepository.Disable(reviewId);
            if (review == null)
                return OperationResult<bool>.Fail($"Reseña con ID {reviewId} no encontrada.");
            
            return OperationResult<bool>.Ok(true, "Reseña eliminada exitosamente.");
        }
        catch (Exception ex)
        {
            return OperationResult<bool>.Fail($"Error al eliminar reseña: {ex.Message}");
        }
    }
}