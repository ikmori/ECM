using ECM.Application.Interfaces.ServicesInterfaces.ProductService;
using ECM.Application.ServicesInterfaces.CategoryService;
using ECM.Domain.Entities.Catalogo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECM.Web.Controllers.Catalogo;

public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    public ProductController(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    // Método auxiliar para cargar el combo de categorías
    private async Task LoadCategoriesViewBagAsync()
    {
        var categoryResult = await _categoryService.GetAllAsync();
        if (categoryResult.Success && categoryResult.Data != null)
        {
            ViewBag.Categories = new SelectList(categoryResult.Data, "Id", "Name");
        }
    }

    // GET: Product/Index
    public async Task<IActionResult> Index()
    {
        var result = await _productService.GetAllAsync();
        
        if (!result.Success)
        {
            ViewBag.InfoMessage = result.Message;
        }

        return View(result.Data);
    }

    // GET: Product/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var result = await _productService.GetByIdAsync(id);
        
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        return View(result.Data);
    }

    // GET: Product/Create
    public async Task<IActionResult> Create()
    {
        await LoadCategoriesViewBagAsync();
        return View();
    }

    // POST: Product/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product product)
    {
        if (!ModelState.IsValid)
        {
            await LoadCategoriesViewBagAsync();
            return View(product);
        }

        var result = await _productService.CreateAsync(product);

        if (result.Success)
        {
            TempData["Success"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        TempData["Error"] = result.Message;
        await LoadCategoriesViewBagAsync();
        return View(product);
    }

    // GET: Product/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var result = await _productService.GetByIdAsync(id);
        
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        await LoadCategoriesViewBagAsync();
        return View(result.Data);
    }

    // POST: Product/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Product product)
    {
        if (id != product.Id)
        {
            TempData["Error"] = "El ID del producto no coincide.";
            return RedirectToAction(nameof(Index));
        }

        if (!ModelState.IsValid)
        {
            await LoadCategoriesViewBagAsync();
            return View(product);
        }

        var result = await _productService.UpdateAsync(product);

        if (result.Success)
        {
            TempData["Success"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        TempData["Error"] = result.Message;
        await LoadCategoriesViewBagAsync();
        return View(product);
    }

    // GET: Product/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _productService.GetByIdAsync(id);
        
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        return View(result.Data);
    }

    // POST: Product/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _productService.DeleteAsync(id);

        if (result.Success)
        {
            TempData["Success"] = result.Message;
        }
        else
        {
            TempData["Error"] = result.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    // POST: Product/UpdateStock
    // Este endpoint extra te permite actualizar solo el stock desde la tabla de inventario
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStock(int id, int quantity)
    {
        var result = await _productService.UpdateStockAsync(id, quantity);

        if (result.Success)
            TempData["Success"] = result.Message;
        else
            TempData["Error"] = result.Message;

        return RedirectToAction(nameof(Index));
    }
    
    [HttpPost]
    public async Task<IActionResult> AddToCart(int productId, int quantity)
    {
        // 1. Llamar a tu lógica de carrito (Sesión, Base de datos o Cookie)
        // 2. Validar stock con el IProductService
    
        TempData["Success"] = "Producto añadido al carrito correctamente.";
        return RedirectToAction("Index", "Product");
    }
}