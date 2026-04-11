

using System.ComponentModel.DataAnnotations;

namespace ECM.Web.Models.Logistica;

public class ReviewViewModel
{
    public int Id { get; set; }
    
    [Display(Name = "Producto")]
    public int ProductId { get; set; }
    
    [Display(Name = "Producto")]
    public string ProductName { get; set; } = string.Empty;
    
    [Display(Name = "Usuario")]
    public int UserId { get; set; }
    
    [Display(Name = "Usuario")]
    public string UserName { get; set; } = string.Empty;
    
    [Display(Name = "Calificación")]
    public int Rating { get; set; }
    
    [Display(Name = "Comentario")]
    public string? Comment { get; set; }
    
    [Display(Name = "Fecha")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
    public DateTime CreatedAt { get; set; }

    // Propiedades calculadas
    public string StarRatingHtml
    {
        get
        {
            var stars = "";
            for (int i = 1; i <= 5; i++)
            {
                if (i <= Rating)
                    stars += "<i class='bi bi-star-fill text-warning'></i>";
                else
                    stars += "<i class='bi bi-star text-muted'></i>";
            }
            return stars;
        }
    }
}

public class CreateReviewViewModel
{
    [Required(ErrorMessage = "El producto es requerido")]
    public int ProductId { get; set; }
    
    [Display(Name = "Producto")]
    public string ProductName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El usuario es requerido")]
    public int UserId { get; set; }

    [Required(ErrorMessage = "La calificación es requerida")]
    [Range(1, 5, ErrorMessage = "La calificación debe ser entre 1 y 5 estrellas")]
    [Display(Name = "Calificación")]
    public int Rating { get; set; } = 5;

    [StringLength(1000, ErrorMessage = "El comentario no puede exceder los 1000 caracteres")]
    [Display(Name = "Comentario")]
    public string? Comment { get; set; }
}

public class EditReviewViewModel
{
    public int Id { get; set; }
    
    public int ProductId { get; set; }
    
    [Display(Name = "Producto")]
    public string ProductName { get; set; } = string.Empty;
    
    public int UserId { get; set; }

    [Required(ErrorMessage = "La calificación es requerida")]
    [Range(1, 5, ErrorMessage = "La calificación debe ser entre 1 y 5 estrellas")]
    [Display(Name = "Calificación")]
    public int Rating { get; set; }

    [StringLength(1000, ErrorMessage = "El comentario no puede exceder los 1000 caracteres")]
    [Display(Name = "Comentario")]
    public string? Comment { get; set; }
}

public class ProductReviewsViewModel
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public double AverageRating { get; set; }
    public int TotalReviews { get; set; }
    public List<ReviewViewModel> Reviews { get; set; } = new List<ReviewViewModel>();
    public Dictionary<int, int> RatingDistribution { get; set; } = new Dictionary<int, int>();
}

public class UserReviewsViewModel
{
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public int TotalReviews { get; set; }
    public List<ReviewViewModel> Reviews { get; set; } = new List<ReviewViewModel>();
}