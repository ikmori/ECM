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

    private async Task LoadCategoriesViewBagAsync()
    {
        var categoryResult = await _categoryService.GetAllAsync();
        // Si el resultado es exitoso, cargamos el SelectList. Si no, enviamos una lista vacía.
        var categories = categoryResult.Success && categoryResult.Data != null 
                         ? categoryResult.Data 
                         : new List<Category>();
        
        ViewBag.Categories = new SelectList(categories, "Id", "Name");
    }

    public async Task<IActionResult> Index()
    {
        var result = await _productService.GetAllAsync();
        
        if (!result.Success)
        {
            ViewBag.InfoMessage = result.Message;
        }

        return View(result.Data ?? new List<Product>());
    }

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

    public async Task<IActionResult> Create()
    {
        await LoadCategoriesViewBagAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product product)
    {
        // IMPORTANTE: Removemos las propiedades de navegación de la validación
        // Esto evita que el ModelState sea falso por culpa de objetos que no vienen en el form
        ModelState.Remove(nameof(product.Category));
        ModelState.Remove(nameof(product.Images));

        if (!ModelState.IsValid)
        {
            // Si llegas aquí, es que falta un campo requerido en el HTML (Nombre, Precio, etc.)
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Product product)
    {
        if (id != product.Id)
        {
            TempData["Error"] = "El ID del producto no coincide.";
            return RedirectToAction(nameof(Index));
        }

        // Al igual que en Create, limpiamos las validaciones de objetos complejos
        ModelState.Remove(nameof(product.Category));
        ModelState.Remove(nameof(product.Images));

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
}