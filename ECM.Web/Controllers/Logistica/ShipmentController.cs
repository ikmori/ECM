// ==========================================
// ECM.Web.Controllers.Logistica.ShipmentController.cs
// ==========================================
using ECM.Application.Services.Logistica;
using ECM.Domain.Common.Enums;
using ECM.Web.Models.Logistica;
using Microsoft.AspNetCore.Mvc;

namespace ECM.Web.Controllers.Logistica;

public class ShipmentController : Controller
{
    private readonly ShipmentService _shipmentService;

    public ShipmentController(ShipmentService shipmentService)
    {
        _shipmentService = shipmentService;
    }

    // GET: Shipment
    public async Task<IActionResult> Index()
    {
        var result = await _shipmentService.GetAllShipmentsAsync();
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            return View(new List<ShipmentViewModel>());
        }

        var viewModels = result.Data.Select(s => new ShipmentViewModel
        {
            Id = s.Id,
            OrderId = s.OrderId,
            TrackingNumber = s.TrackingNumber,
            CarrierName = s.CarrierName,
            Status = s.Status,
            EstimatedDeliveryDate = s.EstimatedDeliveryDate,
            ActualDeliveryDate = s.ActualDeliveryDate,
            CreatedAt = s.CreatedAt
        }).ToList();

        return View(viewModels);
    }

    // GET: Shipment/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var result = await _shipmentService.GetShipmentByIdAsync(id);
        if (!result.Success || result.Data == null)
        {
            TempData["ErrorMessage"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        var shipment = result.Data;
        var viewModel = new ShipmentViewModel
        {
            Id = shipment.Id,
            OrderId = shipment.OrderId,
            TrackingNumber = shipment.TrackingNumber,
            CarrierName = shipment.CarrierName,
            Status = shipment.Status,
            EstimatedDeliveryDate = shipment.EstimatedDeliveryDate,
            ActualDeliveryDate = shipment.ActualDeliveryDate,
            CreatedAt = shipment.CreatedAt
        };

        return View(viewModel);
    }

    // GET: Shipment/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Shipment/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateShipmentViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _shipmentService.CreateShipmentAsync(
                model.OrderId,
                model.TrackingNumber,
                model.CarrierName,
                model.EstimatedDeliveryDate
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

    // GET: Shipment/EditStatus/5
    public async Task<IActionResult> EditStatus(int id)
    {
        var result = await _shipmentService.GetShipmentByIdAsync(id);
        if (!result.Success || result.Data == null)
        {
            TempData["ErrorMessage"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        var shipment = result.Data;
        var viewModel = new EditShipmentStatusViewModel
        {
            Id = shipment.Id,
            TrackingNumber = shipment.TrackingNumber,
            CurrentStatus = shipment.Status,
            NewStatus = shipment.Status
        };

        return View(viewModel);
    }

    // POST: Shipment/EditStatus/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditStatus(int id, EditShipmentStatusViewModel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var result = await _shipmentService.UpdateShipmentStatusAsync(id, model.NewStatus);
            if (result.Success)
            {
                TempData["SuccessMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = result.Message;
        }
        return View(model);
    }

    // GET: Shipment/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _shipmentService.GetShipmentByIdAsync(id);
        if (!result.Success || result.Data == null)
        {
            TempData["ErrorMessage"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        var shipment = result.Data;
        var viewModel = new ShipmentViewModel
        {
            Id = shipment.Id,
            OrderId = shipment.OrderId,
            TrackingNumber = shipment.TrackingNumber,
            CarrierName = shipment.CarrierName,
            Status = shipment.Status
        };

        return View(viewModel);
    }

    // POST: Shipment/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _shipmentService.CancelShipmentAsync(id);
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

    // GET: Shipment/Track
    public IActionResult Track()
    {
        return View(new TrackShipmentViewModel());
    }

    // POST: Shipment/Track
    [HttpPost]
    public async Task<IActionResult> Track(string trackingNumber)
    {
        if (string.IsNullOrEmpty(trackingNumber))
        {
            TempData["ErrorMessage"] = "Ingrese un número de tracking.";
            return View(new TrackShipmentViewModel());
        }

        var result = await _shipmentService.GetShipmentByTrackingNumberAsync(trackingNumber);
        var viewModel = new TrackShipmentViewModel
        {
            TrackingNumber = trackingNumber,
            Shipment = result.Success && result.Data != null ? new ShipmentViewModel
            {
                Id = result.Data.Id,
                OrderId = result.Data.OrderId,
                TrackingNumber = result.Data.TrackingNumber,
                CarrierName = result.Data.CarrierName,
                Status = result.Data.Status,
                EstimatedDeliveryDate = result.Data.EstimatedDeliveryDate,
                ActualDeliveryDate = result.Data.ActualDeliveryDate
            } : null
        };

        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
        }

        return View(viewModel);
    }
}