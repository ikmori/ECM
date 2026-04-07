using ECM.Application.Interfaces.ServicesInterfaces.OrderService;
using ECM.Domain.Common.Enums;
using ECM.Domain.Entities.Ventas;
using ECM.Web.Models.Ventas;
using Microsoft.AspNetCore.Mvc;
namespace ECM.Web.Controllers.Ventas;

public class OrderController : Controller
{
    private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: Order
        public async Task<IActionResult> Index()
        {
            var result = await _orderService.GetAllAsync();
            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
                return View(new List<OrderViewModel>());
            }
            
            var viewModels = result.Data.Select(o => new OrderViewModel
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                UserId = o.UserId
            }).ToList();

            return View(viewModels);
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var result = await _orderService.GetByIdAsync(id);
            if (!result.Success || result.Data == null)
            {
                return NotFound();
            }

            var order = result.Data;
            var viewModel = new OrderViewModel
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                SubTotal = order.SubTotal,
                ShippingCost = order.ShippingCost,
                DiscountAmount = order.DiscountAmount,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                ShippingAddress = order.ShippingAddress,
                UserId = order.UserId,
                OrderItems = order.OrderItems.Select(i => new OrderItemViewModel
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            };

            return View(viewModel);
        }

        
        
        
        
        
        
        
        // POST: Order/AddItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(int orderId, int productId, string productName, int quantity, decimal unitPrice)
        {
            var result = await _orderService.AddOrderItemAsync(orderId, productId, productName, quantity, unitPrice);
    
            if (result.Success)
            {
                TempData["SuccessMessage"] = result.Message;
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
            }
            
            return RedirectToAction(nameof(Details), new { id = orderId });
        }
        
        
        
        
        
        
        
        
        
        
        
        
        // GET: Order/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int userId)
        {
            var result = await _orderService.CreateOrderAsync(userId);
            if (result.Success)
            {
                TempData["SuccessMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", result.Message);
            return View();
        }

        // GET: Order/EditStatus/5
        public async Task<IActionResult> EditStatus(int id)
        {
            var result = await _orderService.GetByIdAsync(id);
            if (!result.Success || result.Data == null)
            {
                return NotFound();
            }

            return View(result.Data); 
        }

        // POST: Order/EditStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditStatus(int id, OrderStatus newStatus)
        {
            var result = await _orderService.ChangeOrderStatusAsync(id, newStatus);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Estado actualizado exitosamente.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", result.Message);
            return RedirectToAction(nameof(EditStatus), new { id });
        }

        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _orderService.GetByIdAsync(id);
            if (!result.Success || result.Data == null)
            {
                return NotFound();
            }

            return View(result.Data);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _orderService.DeleteAsync(id);
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
}