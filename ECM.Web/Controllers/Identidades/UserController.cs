using Microsoft.AspNetCore.Mvc;

namespace ECM.Web.Controllers.Identidades;

public class UserController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}