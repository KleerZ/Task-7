using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Task.Mvc.Controllers;

[Authorize]
public class GameController : Controller
{
    [HttpGet]
    public IActionResult Index(Guid connectionId)
    {
        return View(connectionId);
    }
}