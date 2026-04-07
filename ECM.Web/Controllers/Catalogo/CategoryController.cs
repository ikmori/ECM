using Microsoft.AspNetCore.Mvc;

namespace ECM.Web.Controllers.Catalogo;

public class CategoryController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}