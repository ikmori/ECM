using Microsoft.AspNetCore.Mvc;

namespace ECM.Web.Controllers.Ventas;

public class CartItemController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}