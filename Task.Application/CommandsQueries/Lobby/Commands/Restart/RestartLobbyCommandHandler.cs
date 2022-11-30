using MediatR;
using Microsoft.EntityFrameworkCore;
using Task.Application.CommandsQueries.Lobby.Commands.Connect;
using Task.Application.Common.Data;
using Task.Application.Common.Interfaces;

namespace Task.Application.CommandsQueries.Lobby.Commands.Restart;

public class RestartLobbyCommandHandler : IRequestHandler<RestartLobbyCommand, Unit>
{
    private readonly IApplicationContext _context;
    private readonly IMediator _mediator;

    public RestartLobbyCommandHandler(IApplicationContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(RestartLobbyCommand request,
        CancellationToken cancellationToken)
    {
        var player = _context.Players.FirstOrDefault(p => p.Name == request.PlayerName);

        var lobby = await _context.Lobbies
            .Include(l => l.Players)
            .FirstOrDefaultAsync(l => l.ConnectionId ==
                                      request.ConnectionId, cancellationToken);

        lobby!.Field = new[] { "", "", "", "", "", "", "", "", "" };
        lobby.Status = LobbyStatus.WaitingForPlayers;
        lobby.Players.RemoveRange(0,2);
        lobby.Players = new List<Domain.Player>();
       
        _context.Lobbies.Update(lobby);
        await _context.SaveChangesAsync(cancellationToken);

        if (lobby.Players.Count == 0)
        {
            lobby.Players.Add(player);
        }
        
        if (lobby.Players.Count == 1)
        {
            lobby.Players[0].StepSymbol = lobby.Players[0].StepSymbol == GameSymbols.Cross
                ? GameSymbols.Zero
                : GameSymbols.Cross;
        
            if (lobby.Players.All(p => p.Name != request.PlayerName))
            {
                lobby.Players.Add(player);
            }
        }
        
        if (lobby.Players.Count == 2)
        {
            lobby.Players[0].StepSymbol = lobby.Players[0].StepSymbol == GameSymbols.Cross
                ? GameSymbols.Zero
                : GameSymbols.Cross;
            
            lobby.Players[1].StepSymbol = lobby.Players[0].StepSymbol switch
            {
                GameSymbols.Cross => GameSymbols.Zero,
                GameSymbols.Zero => GameSymbols.Cross,
                _ => lobby.Players[1].StepSymbol
            };
        }

        lobby.PlayerNameStep = lobby.Players
            .FirstOrDefault(p => p.StepSymbol == GameSymbols.Cross)!.Name;

        _context.Lobbies.Update(lobby);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}