// ==========================================
// ECM.Web.Models.Logistica.ShipmentViewModels.cs
// ==========================================
using ECM.Domain.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace ECM.Web.Models.Logistica;

public class ShipmentViewModel
{
    public int Id { get; set; }
    
    [Display(Name = "Orden #")]
    public int OrderId { get; set; }
    
    [Display(Name = "Número de Tracking")]
    public string TrackingNumber { get; set; } = string.Empty;
    
    [Display(Name = "Transportista")]
    public string CarrierName { get; set; } = string.Empty;
    
    [Display(Name = "Estado")]
    public ShipmentStatus Status { get; set; }
    
    [Display(Name = "Fecha Estimada de Entrega")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime? EstimatedDeliveryDate { get; set; }
    
    [Display(Name = "Fecha de Entrega Real")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime? ActualDeliveryDate { get; set; }
    
    [Display(Name = "Fecha de Creación")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
    public DateTime CreatedAt { get; set; }

    // Propiedades calculadas
    public string StatusBadgeClass => Status switch
    {
        ShipmentStatus.Delivered => "success",
        ShipmentStatus.InTransit => "primary",
        ShipmentStatus.Shipped => "info",
        ShipmentStatus.Preparing => "warning",
        ShipmentStatus.Cancelled => "danger",
        _ => "secondary"
    };

    public string StatusIcon => Status switch
    {
        ShipmentStatus.Delivered => "bi-check-circle-fill",
        ShipmentStatus.InTransit => "bi-truck",
        ShipmentStatus.Shipped => "bi-box-seam",
        ShipmentStatus.Preparing => "bi-archive",
        ShipmentStatus.Cancelled => "bi-x-circle-fill",
        _ => "bi-question-circle"
    };

    public bool IsDelivered => Status == ShipmentStatus.Delivered;
    public bool IsCancelled => Status == ShipmentStatus.Cancelled;
}

public class CreateShipmentViewModel
{
    [Required(ErrorMessage = "El ID de la orden es requerido")]
    [Display(Name = "Orden #")]
    public int OrderId { get; set; }

    [Required(ErrorMessage = "El número de tracking es requerido")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "El tracking debe tener entre 3 y 100 caracteres")]
    [Display(Name = "Número de Tracking")]
    public string TrackingNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre del transportista es requerido")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
    [Display(Name = "Transportista")]
    public string CarrierName { get; set; } = string.Empty;

    [Display(Name = "Fecha Estimada de Entrega")]
    [DataType(DataType.Date)]
    public DateTime? EstimatedDeliveryDate { get; set; }
}

public class EditShipmentStatusViewModel
{
    public int Id { get; set; }
    
    [Display(Name = "Número de Tracking")]
    public string TrackingNumber { get; set; } = string.Empty;
    
    [Display(Name = "Estado Actual")]
    public ShipmentStatus CurrentStatus { get; set; }
    
    [Required(ErrorMessage = "El nuevo estado es requerido")]
    [Display(Name = "Nuevo Estado")]
    public ShipmentStatus NewStatus { get; set; }
}

public class TrackShipmentViewModel
{
    [Required(ErrorMessage = "El número de tracking es requerido")]
    [Display(Name = "Número de Tracking")]
    public string TrackingNumber { get; set; } = string.Empty;

    public ShipmentViewModel? Shipment { get; set; }
}