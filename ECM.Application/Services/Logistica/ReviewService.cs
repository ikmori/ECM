using ECM.Domain.Entities.Logistica;

namespace ECM.Application.Services.Logistica;

public class ReviewService
{
    private readonly List<Review> _reviews = new();

    public Review AddReview(int productId, int userId, int rating, string? comment)
    {
        var review = new Review
        {
            Id = _reviews.Count + 1,
            ProductId = productId,
            UserId = userId,
            Rating = rating,
            Comment = comment
        };

        _reviews.Add(review);
        return review;
    }

    public List<Review> GetProductReviews(int productId)
    {
        return _reviews.Where(r => r.ProductId == productId).ToList();
    }

    public double GetAverageRating(int productId)
    {
        return _reviews
            .Where(r => r.ProductId == productId)
            .Select(r => (double)r.Rating)
            .DefaultIfEmpty(0)
            .Average();
    }

    public bool DeleteReview(int reviewId, int userId)
    {
        var review = _reviews.FirstOrDefault(r => r.Id == reviewId && r.UserId == userId);
        if (review == null)
            return false;

        return _reviews.Remove(review);
    }
}