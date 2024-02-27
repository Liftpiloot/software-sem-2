using Microsoft.AspNetCore.Mvc;

namespace IronApp.Controllers;

public class LoginController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}