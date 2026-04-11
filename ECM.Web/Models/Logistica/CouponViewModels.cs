
using System.ComponentModel.DataAnnotations;
using ECM.Domain.Common.Enums;

namespace ECM.Web.Models.Logistica;

public class CouponViewModel
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public DiscountType Type { get; set; }
    public decimal Value { get; set; }
    public decimal? MinimumOrderAmount { get; set; }
    public DateTime ExpirationDate { get; set; }
    public int MaxUsage { get; set; }
    public int TimesUsed { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Propiedades calculadas para la vista
    public string FormattedValue => Type == DiscountType.Percentage ? $"{Value}%" : $"{Value:C}";
    public string FormattedMinimumAmount => MinimumOrderAmount.HasValue ? $"{MinimumOrderAmount.Value:C}" : "Sin mínimo";
    public string StatusBadgeClass => IsActive && ExpirationDate > DateTime.UtcNow && TimesUsed < MaxUsage ? "success" : "danger";
    public string StatusText => IsActive && ExpirationDate > DateTime.UtcNow && TimesUsed < MaxUsage ? "Activo" : "Inactivo";
    public int UsagePercentage => MaxUsage > 0 ? (TimesUsed * 100) / MaxUsage : 0;
    public bool IsExpired => ExpirationDate <= DateTime.UtcNow;
    public bool IsExhausted => TimesUsed >= MaxUsage;
}

public class CreateCouponViewModel
{
    [Required(ErrorMessage = "El código del cupón es requerido")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "El código debe tener entre 3 y 50 caracteres")]
    [Display(Name = "Código del Cupón")]
    public string Code { get; set; } = string.Empty;

    [Required(ErrorMessage = "El tipo de descuento es requerido")]
    [Display(Name = "Tipo de Descuento")]
    public DiscountType Type { get; set; }

    [Required(ErrorMessage = "El valor del descuento es requerido")]
    [Range(0.01, 999999.99, ErrorMessage = "El valor debe ser mayor a 0")]
    [Display(Name = "Valor")]
    public decimal Value { get; set; }

    [Display(Name = "Monto Mínimo de Compra")]
    [Range(0, 999999.99, ErrorMessage = "El monto mínimo debe ser un valor positivo")]
    public decimal? MinimumOrderAmount { get; set; }

    [Required(ErrorMessage = "La fecha de expiración es requerida")]
    [Display(Name = "Fecha de Expiración")]
    [DataType(DataType.Date)]
    public DateTime ExpirationDate { get; set; } = DateTime.UtcNow.AddMonths(1);

    [Required(ErrorMessage = "El número máximo de usos es requerido")]
    [Range(1, 999999, ErrorMessage = "El número máximo de usos debe ser mayor a 0")]
    [Display(Name = "Usos Máximos")]
    public int MaxUsage { get; set; } = 100;
}

public class EditCouponViewModel
{
    public int Id { get; set; }

    [Display(Name = "Código")]
    public string Code { get; set; } = string.Empty;

    [Display(Name = "Tipo")]
    public DiscountType Type { get; set; }

    [Required(ErrorMessage = "El valor del descuento es requerido")]
    [Range(0.01, 999999.99, ErrorMessage = "El valor debe ser mayor a 0")]
    [Display(Name = "Valor")]
    public decimal Value { get; set; }

    [Display(Name = "Monto Mínimo de Compra")]
    [Range(0, 999999.99, ErrorMessage = "El monto mínimo debe ser un valor positivo")]
    public decimal? MinimumOrderAmount { get; set; }

    [Required(ErrorMessage = "La fecha de expiración es requerida")]
    [Display(Name = "Fecha de Expiración")]
    [DataType(DataType.Date)]
    public DateTime ExpirationDate { get; set; }

    [Required(ErrorMessage = "El número máximo de usos es requerido")]
    [Range(1, 999999, ErrorMessage = "El número máximo de usos debe ser mayor a 0")]
    [Display(Name = "Usos Máximos")]
    public int MaxUsage { get; set; }
}

public class ValidateCouponViewModel
{
    [Required(ErrorMessage = "El código del cupón es requerido")]
    [Display(Name = "Código del Cupón")]
    public string Code { get; set; } = string.Empty;

    [Required(ErrorMessage = "El monto de la orden es requerido")]
    [Range(0.01, 999999.99, ErrorMessage = "El monto debe ser mayor a 0")]
    [Display(Name = "Monto de la Orden")]
    public decimal OrderAmount { get; set; }

    public bool IsValid { get; set; }
    public decimal? DiscountAmount { get; set; }
    public string? DiscountType { get; set; }
    public string? Message { get; set; }
}