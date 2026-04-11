// ==========================================
// ECM.Web.Controllers.Logistica.ReviewController.cs
// ==========================================
using ECM.Application.Services.Logistica;
using ECM.Web.Models.Logistica;
using Microsoft.AspNetCore.Mvc;

namespace ECM.Web.Controllers.Logistica;

public class ReviewController : Controller
{
    private readonly ReviewService _reviewService;

    public ReviewController(ReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    // GET: Review
    public async Task<IActionResult> Index()
    {
        var result = await _reviewService.GetAllReviewsAsync();
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            return View(new List<ReviewViewModel>());
        }

        var viewModels = result.Data.Select(r => new ReviewViewModel
        {
            Id = r.Id,
            ProductId = r.ProductId,
            ProductName = $"Producto #{r.ProductId}",
            UserId = r.UserId,
            UserName = $"Usuario #{r.UserId}",
            Rating = r.Rating,
            Comment = r.Comment,
            CreatedAt = r.CreatedAt
        }).ToList();

        return View(viewModels);
    }

    // GET: Review/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var result = await _reviewService.GetReviewByIdAsync(id);
        if (!result.Success || result.Data == null)
        {
            TempData["ErrorMessage"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        var review = result.Data;
        var viewModel = new ReviewViewModel
        {
            Id = review.Id,
            ProductId = review.ProductId,
            ProductName = $"Producto #{review.ProductId}",
            UserId = review.UserId,
            UserName = $"Usuario #{review.UserId}",
            Rating = review.Rating,
            Comment = review.Comment,
            CreatedAt = review.CreatedAt
        };

        return View(viewModel);
    }

    // GET: Review/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Review/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateReviewViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _reviewService.CreateReviewAsync(
                model.ProductId,
                model.UserId,
                model.Rating,
                model.Comment
            );

            if (result.Success)
            {
                TempData["SuccessMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = result.Message;
        }
        return View(model);
    }

    // GET: Review/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var result = await _reviewService.GetReviewByIdAsync(id);
        if (!result.Success || result.Data == null)
        {
            TempData["ErrorMessage"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        var review = result.Data;
        var viewModel = new EditReviewViewModel
        {
            Id = review.Id,
            ProductId = review.ProductId,
            ProductName = $"Producto #{review.ProductId}",
            UserId = review.UserId,
            Rating = review.Rating,
            Comment = review.Comment
        };

        return View(viewModel);
    }

    // POST: Review/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EditReviewViewModel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var result = await _reviewService.UpdateReviewAsync(id, model.Rating, model.Comment);
            if (result.Success)
            {
                TempData["SuccessMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = result.Message;
        }
        return View(model);
    }

    // GET: Review/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _reviewService.GetReviewByIdAsync(id);
        if (!result.Success || result.Data == null)
        {
            TempData["ErrorMessage"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        var review = result.Data;
        var viewModel = new ReviewViewModel
        {
            Id = review.Id,
            ProductId = review.ProductId,
            ProductName = $"Producto #{review.ProductId}",
            UserId = review.UserId,
            UserName = $"Usuario #{review.UserId}",
            Rating = review.Rating,
            Comment = review.Comment
        };

        return View(viewModel);
    }

    // POST: Review/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _reviewService.DeleteReviewAsync(id);
        if (result.Success)
        {
            TempData["SuccessMessage"] = result.Message;
        }
        else
        {
            TempData["ErrorMessage"] = result.Message;
        }
        return RedirectToAction(nameof(Index));
    }

    // GET: Review/ProductReviews/5
    public async Task<IActionResult> ProductReviews(int productId, string productName = "")
    {
        var result = await _reviewService.GetReviewsByProductIdAsync(productId);
        var averageResult = await _reviewService.GetAverageRatingAsync(productId);
        
        var viewModel = new ProductReviewsViewModel
        {
            ProductId = productId,
            ProductName = string.IsNullOrEmpty(productName) ? $"Producto #{productId}" : productName,
            AverageRating = averageResult.Success ? averageResult.Data : 0,
            TotalReviews = result.Success ? result.Data.Count() : 0,
            Reviews = result.Success ? result.Data.Select(r => new ReviewViewModel
            {
                Id = r.Id,
                ProductId = r.ProductId,
                ProductName = $"Producto #{r.ProductId}",
                UserId = r.UserId,
                UserName = $"Usuario #{r.UserId}",
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt
            }).ToList() : new List<ReviewViewModel>()
        };

        // Calcular distribución de calificaciones
        viewModel.RatingDistribution = new Dictionary<int, int>();
        for (int i = 1; i <= 5; i++)
        {
            var count = viewModel.Reviews.Count(r => r.Rating == i);
            viewModel.RatingDistribution[i] = count;
        }

        return View(viewModel);
    }

    // GET: Review/UserReviews/5
    public async Task<IActionResult> UserReviews(int userId, string userName = "")
    {
        var result = await _reviewService.GetReviewsByUserIdAsync(userId);
        
        var viewModel = new UserReviewsViewModel
        {
            UserId = userId,
            UserName = string.IsNullOrEmpty(userName) ? $"Usuario #{userId}" : userName,
            TotalReviews = result.Success ? result.Data.Count() : 0,
            Reviews = result.Success ? result.Data.Select(r => new ReviewViewModel
            {
                Id = r.Id,
                ProductId = r.ProductId,
                ProductName = $"Producto #{r.ProductId}",
                UserId = r.UserId,
                UserName = $"Usuario #{r.UserId}",
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt
            }).ToList() : new List<ReviewViewModel>()
        };

        return View(viewModel);
    }
}