using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Task.Application.CommandsQueries.Player.Commands.Login;
using Task.Mvc.Models;

namespace Task.Mvc.Controllers;

public class LoginController : Controller
{
    private readonly IMediator _mediator;

    public LoginController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Index(PlayerViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var command = new PlayerLoginCommand { Name = model.Name };
        var identity = await _mediator.Send(command);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
            new ClaimsPrincipal(identity));

        return RedirectToAction("Index", "Home");
    }
}