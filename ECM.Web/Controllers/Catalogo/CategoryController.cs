using ECM.Application.ServicesInterfaces.CategoryService;
using ECM.Domain.Entities.Catalogo;
using Microsoft.AspNetCore.Mvc;

namespace ECM.Web.Controllers.Catalogo;

public class CategoryController : Controller
{
    // GET
   private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // GET: Category/Index
    public async Task<IActionResult> Index()
    {
        var result = await _categoryService.GetAllAsync();
        
        if (!result.Success)
        {
            ViewBag.InfoMessage = result.Message;
        }

        return View(result.Data);
    }

    // GET: Category/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var result = await _categoryService.GetByIdAsync(id);
        
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        return View(result.Data);
    }

    // GET: Category/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Category/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Category category)
    {
        if (!ModelState.IsValid) return View(category);

        var result = await _categoryService.CreateAsync(category);

        if (result.Success)
        {
            TempData["Success"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        TempData["Error"] = result.Message;
        return View(category);
    }

    // GET: Category/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var result = await _categoryService.GetByIdAsync(id);
        
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        return View(result.Data);
    }

    // POST: Category/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Category category)
    {
        if (id != category.Id)
        {
            TempData["Error"] = "El ID de la categoría no coincide.";
            return RedirectToAction(nameof(Index));
        }

        if (!ModelState.IsValid) return View(category);

        var result = await _categoryService.UpdateAsync(category);

        if (result.Success)
        {
            TempData["Success"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        TempData["Error"] = result.Message;
        return View(category);
    }

    // GET: Category/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _categoryService.GetByIdAsync(id);
        
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        return View(result.Data);
    }

    // POST: Category/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _categoryService.DeleteAsync(id);

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
}