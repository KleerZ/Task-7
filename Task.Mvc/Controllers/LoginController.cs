using Microsoft.AspNetCore.Mvc;

namespace Task.Mvc.Controllers;

public class LoginController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}