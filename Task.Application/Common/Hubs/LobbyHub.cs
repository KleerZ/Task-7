using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using Task.Application.CommandsQueries.Lobby.Commands.ChangeLobbyStatus;
using Task.Application.CommandsQueries.Lobby.Commands.CheckWin;
using Task.Application.CommandsQueries.Lobby.Commands.Connect;
using Task.Application.CommandsQueries.Lobby.Commands.Restart;
using Task.Application.CommandsQueries.Lobby.Commands.Step;
using Task.Application.CommandsQueries.Lobby.Queries.CheckPlayersCount;
using Task.Application.CommandsQueries.Lobby.Queries.GetField;
using Task.Application.CommandsQueries.Lobby.Queries.GetPlayerNameStep;
using Task.Application.CommandsQueries.Lobby.Queries.GetPlayers;
using Task.Application.CommandsQueries.Lobby.Queries.GetStatus;
using Task.Application.CommandsQueries.Player.Queries.GetPlayerStepSymbol;
using Task.Application.Common.Data;
using Task.Application.Common.Interfaces;

namespace Task.Application.Common.Hubs;

[Authorize]
public class LobbyHub : Hub
{
    private readonly IMediator _mediator;
    private readonly IApplicationContext _context;

    public LobbyHub(IMediator mediator, IApplicationContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    public async System.Threading.Tasks.Task GetField(string connectionId, string playerName)
    {
        var query = new GetLobbyFieldQuery
        {
            ConnectionId = connectionId
        };

        var field = await _mediator.Send(query);

        await Clients.User(playerName).SendAsync("GetField", field);
    }

    public async System.Threading.Tasks.Task CheckPlayers(string connectionId, string playerName)
    {
        var queryPlayers = new GetLobbyPlayersQuery
        {
            ConnectionId = Guid.Parse(connectionId)
        };

        var players = await _mediator.Send(queryPlayers);

        var query = new CheckPlayersCountQuery
        {
            ConnectionId = connectionId,
            PlayerName = playerName
        };

        var result = await _mediator.Send(query);

        var message = new LobbyMessageVm
        {
            PlayersCount = result.PlayersCount
        };

        foreach (var player in players.Players!)
        {
            await Clients.User(player.Name).SendAsync("CheckPlayers", message);
            await GetLobbyResult(connectionId, player.Name);
            await GetField(connectionId, player.Name);
        }
    }

    public async System.Threading.Tasks.Task GetStepSymbol(string playerName)
    {
        var query = new GetPlayerStepSymbolQuery
        {
            PlayerName = playerName
        };

        var stepSymbol = await _mediator.Send(query);

        await Clients.User(playerName).SendAsync("GetStepSymbol", stepSymbol);
    }

    public async System.Threading.Tasks.Task GetPlayerNameStep(string connectionId)
    {
        var queryPlayers = new GetLobbyPlayersQuery
        {
            ConnectionId = Guid.Parse(connectionId)
        };

        var players = await _mediator.Send(queryPlayers);

        var query = new GetPlayerNameStepQuery
        {
            ConnectionId = Guid.Parse(connectionId)
        };

        var stepPlayerName = await _mediator.Send(query);

        foreach (var player in players.Players!)
            await Clients.User(player.Name).SendAsync("GetPlayerNameStep", stepPlayerName);
    }

    public async System.Threading.Tasks.Task Step(string connectionId, string playerName,
        string symbol, int cellIndex)
    {
        var query = new GetLobbyPlayersQuery
        {
            ConnectionId = Guid.Parse(connectionId)
        };

        var players = await _mediator.Send(query);

        var command = new LobbyStepCommand
        {
            ConnectionId = connectionId,
            Symbol = symbol,
            CellIndex = cellIndex
        };

        var cells = await _mediator.Send(command);
        
        foreach (var player in players.Players!)
        {
            await GetPlayerNameStep(connectionId);
            await Clients.User(player.Name).SendAsync("Step", cells);
            await GetLobbyResult(connectionId, player.Name);
        }
    }

    public async System.Threading.Tasks.Task GetLobbyResult(string connectionId, string player)
    {
        var commandWinner= new CheckLobbyWinCommand
        {
            ConnectionId = connectionId
        };

        var winner = await _mediator.Send(commandWinner);
        
        await Clients.User(player).SendAsync("GetLobbyResult", winner);
    }

    public async System.Threading.Tasks.Task GetLobbyStatus(string connectionId)
    {
        var queryPlayers = new GetLobbyPlayersQuery
        {
            ConnectionId = Guid.Parse(connectionId)
        };

        var players = await _mediator.Send(queryPlayers);
        
        var query = new GetLobbyStatusQuery
        {
            ConnectionId = connectionId
        };

        var status = await _mediator.Send(query);

        foreach (var player in players.Players!)
        {
            await Clients.User(player.Name).SendAsync("GetLobbyStatus", status);
            await GetLobbyResult(connectionId, player.Name);
        }
    }

    public async System.Threading.Tasks.Task Restart(Guid connectionId, string playerName)
    {
        var command = new RestartLobbyCommand
        {
            ConnectionId = connectionId,
            PlayerName = playerName
        };
        
        await _mediator.Send(command);
        
        var queryPlayers = new GetLobbyPlayersQuery
        {
            ConnectionId = connectionId
        };

        var players = await _mediator.Send(queryPlayers);
        
        if (players.Players.Count() == 2)
        {
            foreach (var p in players.Players)
            {
                await Clients.User(p.Name).SendAsync("Restart");
                await CheckPlayers(connectionId.ToString(), p.Name);
                await GetField(connectionId.ToString(), p.Name);
                await GetPlayerNameStep(connectionId.ToString());
                await GetLobbyStatus(connectionId.ToString());
                await CheckPlayers(connectionId.ToString(), p.Name);
            }
        }

        await Clients.User(playerName).SendAsync("Restart");
        await CheckPlayers(connectionId.ToString(), playerName);
        await GetField(connectionId.ToString(), playerName);
        await GetPlayerNameStep(connectionId.ToString());
        await GetLobbyStatus(connectionId.ToString());
        await CheckPlayers(connectionId.ToString(), playerName);
        

    }

    public async System.Threading.Tasks.Task SendLobbyInfo(Guid connectionId, string status)
    {
        var command = new ChangeLobbyStatusCommand
        {
            Status = status,
            ConnectionId = connectionId
        };

        await _mediator.Send(command);
        
        await Clients.All.SendAsync("SendLobbyInfo");
    }
    
    public async System.Threading.Tasks.Task Connect(Guid connectionId, string player)
    {
        var command = new ConnectToLobbyCommand
        {
            PlayerName = player,
            ConnectionId = connectionId
        };

        await _mediator.Send(command);
        
        await Clients.User(player).SendAsync("Connect");
        await CheckPlayers(connectionId.ToString(), player);
        await GetField(connectionId.ToString(), player);
        await GetPlayerNameStep(connectionId.ToString());
        await GetLobbyStatus(connectionId.ToString());
    }
}