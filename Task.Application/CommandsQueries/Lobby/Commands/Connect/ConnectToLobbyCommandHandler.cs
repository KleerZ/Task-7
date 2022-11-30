using MediatR;
using Microsoft.EntityFrameworkCore;
using Task.Application.Common.Data;
using Task.Application.Common.Interfaces;

namespace Task.Application.CommandsQueries.Lobby.Commands.Connect;

public class ConnectToLobbyCommandHandler : IRequestHandler<ConnectToLobbyCommand, ConnectLobbyVm>
{
    private readonly IApplicationContext _context;

    public ConnectToLobbyCommandHandler(IApplicationContext context)
    {
        _context = context;
    }

    public async Task<ConnectLobbyVm> Handle(ConnectToLobbyCommand request,
        CancellationToken cancellationToken)
    {
        var player = await _context.Players
            .FirstOrDefaultAsync(p => p.Name == request.PlayerName, cancellationToken);

        if (player is null)
            throw new NullReferenceException($"The player '{request.PlayerName}' does not exist");

        var lobby = await _context.Lobbies
            .Include(p => p.Players)
            .FirstOrDefaultAsync(l => l.ConnectionId == request.ConnectionId, cancellationToken);

        if (lobby is null)
        {
            request.ModelState.AddModelError("lobby-error", "the lobby does not exist");
            return new ConnectLobbyVm { ModelState = request.ModelState };
        }

        if (lobby.Players.Count >= 2 && lobby.Players.All(p => p.Name != request.PlayerName))
        {
            lobby.Status = LobbyStatus.Occupied;
            request.ModelState.AddModelError("lobby-occupied", "the lobby is occupied");
            return new ConnectLobbyVm { ModelState = request.ModelState };
        }
        
        lobby.Players.Add(player);

        if (lobby.Players.IndexOf(player) == 1)
            player.StepSymbol = "O";

        if (lobby.Players.Count > 1)
        {
            lobby.PlayerNameStep = lobby.Players
                .FirstOrDefault(p => p.StepSymbol == GameSymbols.Cross)!.Name;
        }

        _context.Lobbies.Update(lobby);
        await _context.SaveChangesAsync(cancellationToken);

        return new ConnectLobbyVm
        {
            ConnectionId = lobby.ConnectionId,
            ModelState = request.ModelState
        };
    }
}