using MediatR;
using Microsoft.EntityFrameworkCore;
using Task.Application.Common.Interfaces;

namespace Task.Application.CommandsQueries.Lobby.Commands.Step;

public class LobbyStepCommandHandler : IRequestHandler<LobbyStepCommand, string[]>
{
    private readonly IApplicationContext _context;

    public LobbyStepCommandHandler(IApplicationContext context)
    {
        _context = context;
    }

    public async Task<string[]> Handle(LobbyStepCommand request,
        CancellationToken cancellationToken)
    {
        var lobby = await _context.Lobbies
            .Include(l => l.Players)
            .FirstOrDefaultAsync(l => l.ConnectionId ==
                                      Guid.Parse(request.ConnectionId!), cancellationToken);

        lobby!.Field![request.CellIndex] = request.Symbol!;

        lobby.PlayerNameStep = lobby.PlayerNameStep == lobby?.Players[0].Name 
            ? lobby?.Players[1].Name 
            : lobby?.Players[0].Name;

        await _context.SaveChangesAsync(cancellationToken);

        return lobby.Field;
    }
}