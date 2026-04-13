using ECM.Application.Interfaces.ServicesInterfaces.ProductService;
using ECM.Application.Interfaces.ServicesInterfaces.ShoppingCartServices;
using ECM.Web.Models.Ventas;
using Microsoft.AspNetCore.Mvc;


namespace ECM.Web.Controllers.Ventas;

public class ShoppingCartController : Controller
{
    private readonly IShoppingCartService _shoppingCartService;
    private readonly ICartItemService _cartItemService;
    private readonly IProductService _productService; 

    public ShoppingCartController(
        IShoppingCartService shoppingCartService,
        ICartItemService cartItemService,
        IProductService productService) 
    {
        _shoppingCartService = shoppingCartService;
        _cartItemService = cartItemService;
        _productService = productService;
    }

    private (int? UserId, string? GuestId) GetUserContext() => (1, null);

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

        foreach (var item in cart.Items)
        {
            var productResult = await _productService.GetByIdAsync(item.ProductId);

            if (productResult.Success && productResult.Data != null)
            {
                viewModel.Items.Add(new CartItemViewModel
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    ProductName = productResult.Data.Name, 
                    UnitPrice = productResult.Data.Price 
                });
            }
            else
            {
                viewModel.Items.Add(new CartItemViewModel
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    ProductName = "Producto no disponible",
                    UnitPrice = 0m
                });
            }
        }

        var allProductsResult = await _productService.GetAllAsync();
        if (allProductsResult.Success && allProductsResult.Data != null)
        {
            ViewBag.AvailableProducts = allProductsResult.Data;
        }
        else
        {
            ViewBag.AvailableProducts = new List<ECM.Domain.Entities.Catalogo.Product>(); 
        }

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddItem(int productId, int quantity)
    {
        // --- VALIDACIÓN DE STOCK EN CONTROLADOR ---
        var productResult = await _productService.GetByIdAsync(productId);
        if (productResult.Success && productResult.Data != null)
        {
            if (quantity > productResult.Data.StockQuantity)
            {
                TempData["Error"] = $"Stock insuficiente. Solo quedan {productResult.Data.StockQuantity} unidades de {productResult.Data.Name}.";
                return RedirectToAction(nameof(Index));
            }
        }
        // ------------------------------------------

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
        // --- VALIDACIÓN DE STOCK AL ACTUALIZAR ---
        var (userId, guestId) = GetUserContext();
        var cartResult = await _shoppingCartService.GetOrCreateCartAsync(userId, guestId);
        var itemInCart = cartResult.Data?.Items.FirstOrDefault(i => i.Id == cartItemId);

        if (itemInCart != null)
        {
            var productResult = await _productService.GetByIdAsync(itemInCart.ProductId);
            if (productResult.Success && productResult.Data != null)
            {
                if (newQuantity > productResult.Data.StockQuantity)
                {
                    TempData["Error"] = $"No puedes subir a {newQuantity}. Solo hay {productResult.Data.StockQuantity} disponibles.";
                    return RedirectToAction(nameof(Index));
                }
            }
        }
        // ------------------------------------------

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