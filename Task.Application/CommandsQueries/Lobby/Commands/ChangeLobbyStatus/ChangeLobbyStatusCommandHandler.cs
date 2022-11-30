using MediatR;
using Microsoft.EntityFrameworkCore;
using Task.Application.Common.Interfaces;

namespace Task.Application.CommandsQueries.Lobby.Commands.ChangeLobbyStatus;

public class ChangeLobbyStatusCommandHandler : IRequestHandler<ChangeLobbyStatusCommand, Unit>
{
    private readonly IApplicationContext _context;

    public ChangeLobbyStatusCommandHandler(IApplicationContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(ChangeLobbyStatusCommand request,
        CancellationToken cancellationToken)
    {
        var lobby = await _context.Lobbies
            .Include(p => p.Players)
            .FirstOrDefaultAsync(l => l.ConnectionId == request.ConnectionId, cancellationToken);

        lobby!.Status = request.Status!;

        _context.Lobbies.Update(lobby);
        await _context.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}