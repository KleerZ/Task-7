using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task.Application.CommandsQueries.Lobby.Commands.Connect;
using Task.Application.CommandsQueries.Lobby.Commands.Create;
using Task.Application.CommandsQueries.Lobby.Queries.GetFree;
using Task.Mvc.Models;

namespace Task.Mvc.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly IMediator _mediator;

    public HomeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var query = new GetFreeLobbiesQuery();
        var lobbies = await _mediator.Send(query);

        var model = new LobbyViewModel
        {
            Lobbies = lobbies.Lobbies
        };
        
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> CreateLobby(LobbyViewModel model)
    {
        var command = new CreateLobbyCommand
        {
            PlayerName = model.PlayerName
        };

        var connectionId = await _mediator.Send(command);

        return RedirectToAction("Index", "Game", new { connectionId = connectionId });
    }
    
    [HttpPost]
    public async Task<IActionResult> ConnectToLobby(LobbyViewModel model)
    {
        var command = new ConnectToLobbyCommand
        {
            PlayerName = model.PlayerName,
            ConnectionId = model.ConnectionId,
            ModelState = ModelState
        };

        var result = await _mediator.Send(command);

        if (!result.ModelState.IsValid)
        {
            var query = new GetFreeLobbiesQuery();
            var lobbies = await _mediator.Send(query);
            
            model.Lobbies = lobbies.Lobbies;
            
            return View("Index", model);
        }

        return RedirectToAction("Index", "Game", new { connectionId = result.ConnectionId });
    }
}