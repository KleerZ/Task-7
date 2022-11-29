using MediatR;
using Microsoft.EntityFrameworkCore;
using Task.Application.Common.Data;
using Task.Application.Common.Interfaces;

namespace Task.Application.CommandsQueries.Lobby.Commands.Restart;

public class RestartLobbyCommandHandler : IRequestHandler<RestartLobbyCommand, Unit>
{
    private readonly IApplicationContext _context;

    public RestartLobbyCommandHandler(IApplicationContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(RestartLobbyCommand request,
        CancellationToken cancellationToken)
    {
        var lobby = await _context.Lobbies
            .Include(l => l.Players)
            .FirstOrDefaultAsync(l => l.ConnectionId ==
                                      request.ConnectionId, cancellationToken);

        lobby!.Field = new[] { "", "", "", "", "", "", "", "", "" };

        var players = lobby.Players.ToArray();

        foreach (var player in players)
        {
            player.StepSymbol = player.StepSymbol == GameSymbols.Cross
                ? GameSymbols.Zero
                : GameSymbols.Cross;
        }

        lobby.PlayerNameStep = lobby.PlayerNameStep == lobby?.Players[0].Name
            ? lobby?.Players[1].Name
            : lobby?.Players[0].Name;

        lobby!.Status = LobbyStatus.Occupied;
        
        _context.Players.UpdateRange(players);
        _context.Lobbies.Update(lobby);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}