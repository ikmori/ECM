using Microsoft.AspNetCore.Mvc;

namespace ECM.Web.Controllers.Ventas;

public class ShoppingCartController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}