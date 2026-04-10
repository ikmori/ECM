using ECM.Application.Interfaces.ServicesInterfaces.ShoppingCartServices;
using ECM.Web.Models.Ventas;
using Microsoft.AspNetCore.Mvc;

namespace ECM.Web.Controllers.Ventas;

public class ShoppingCartController : Controller
{
   private readonly IShoppingCartService _shoppingCartService;
    private readonly ICartItemService _cartItemService;

    public ShoppingCartController(
        IShoppingCartService shoppingCartService,
        ICartItemService cartItemService)
    {
        _shoppingCartService = shoppingCartService;
        _cartItemService = cartItemService;
    }

    private (int? UserId, string? GuestId) GetUserContext() => (1, null);

    // GET: ShoppingCart
    public async Task<IActionResult> Index()
    {
        var (userId, guestId) = GetUserContext();
        var result = await _shoppingCartService.GetOrCreateCartAsync(userId, guestId);

        if (!result.Success || result.Data == null)
        {
            TempData["Error"] = result.Message;
            return View(new CartViewModel());
        }

        var cart = result.Data;
        var viewModel = new CartViewModel { Id = cart.Id };

        // 1. Mapeo de productos 
        foreach (var item in cart.Items)
        {
            viewModel.Items.Add(new CartItemViewModel
            {
                Id = item.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                ProductName = $"Producto #{item.ProductId}",
                UnitPrice = 50.00m 
            });
        }

        //  --- PRODUCTOS FICTICIOS PARA PRUEBAS 
        /*
        if (!viewModel.Items.Any(i => i.ProductId == 999)) 
        {
            viewModel.Items.Add(new CartItemViewModel
            {
                Id = 999, // ID ficticio para el item
                ProductId = 101,
                ProductName = "Laptop Gaming Pro (Prueba)",
                UnitPrice = 1200.00m,
                Quantity = 1
            });

            viewModel.Items.Add(new CartItemViewModel
            {
                Id = 888,
                ProductId = 102,
                ProductName = "Mouse Inalámbrico (Prueba)",
                UnitPrice = 45.50m,
                Quantity = 2
            });
        }
        */

        return View(viewModel);
    }

    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddItem(int productId, int quantity)
    {
        var (userId, guestId) = GetUserContext();
        var result = await _cartItemService.AddItemAsync(userId, guestId, productId, quantity);

        if (result.Success) TempData["Success"] = result.Message;
        else TempData["Error"] = result.Message;

        return RedirectToAction(nameof(Index));
    }

    
    
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateQuantity(int cartItemId, int newQuantity)
    {
        var (userId, guestId) = GetUserContext();
        var result = await _cartItemService.UpdateQuantityAsync(userId, guestId, cartItemId, newQuantity);

        if (result.Success) TempData["Success"] = result.Message;
        else TempData["Error"] = result.Message;

        return RedirectToAction(nameof(Index));
    }
    
    

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveItem(int cartItemId)
    {
        var (userId, guestId) = GetUserContext();
        var result = await _cartItemService.RemoveItemAsync(userId, guestId, cartItemId);

        if (result.Success) TempData["Success"] = result.Message;
        else TempData["Error"] = result.Message;

        return RedirectToAction(nameof(Index));
    }

    
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ClearCart()
    {
        var (userId, guestId) = GetUserContext();
        var result = await _shoppingCartService.ClearCartAsync(userId, guestId);

        if (result.Success) TempData["Success"] = result.Message;
        else TempData["Error"] = result.Message;

        return RedirectToAction(nameof(Index));
    }
}