using Microsoft.AspNetCore.Mvc;

namespace ECM.Web.Controllers.Logistica;

public class CouponController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}