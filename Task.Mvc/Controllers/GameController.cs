using Microsoft.AspNetCore.Mvc;

namespace Task.Mvc.Controllers;

public class GameController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}