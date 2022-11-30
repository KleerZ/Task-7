using MediatR;
using Microsoft.EntityFrameworkCore;
using Task.Application.Common.Data;
using Task.Application.Common.Interfaces;

namespace Task.Application.CommandsQueries.Lobby.Commands.Leave;

public class LeaveLobbyCommandHandler : IRequestHandler<LeaveLobbyCommand, Domain.Lobby>
{
    private readonly IApplicationContext _context;

    public LeaveLobbyCommandHandler(IApplicationContext context)
    {
        _context = context;
    }

    public async Task<Domain.Lobby> Handle(LeaveLobbyCommand request, CancellationToken cancellationToken)
    {
        var lobby = await _context.Lobbies
            .Include(l => l.Players)
            .FirstOrDefaultAsync(p => p.ConnectionId == request.ConnectionId, cancellationToken);

        if (lobby is null)
            throw new NullReferenceException($"The lobby does not exist");

        var removedPlayer = lobby.Players
            .First(p => p.Name == request.PlayerName);

        lobby.Players.Remove(removedPlayer);
        lobby.Status = LobbyStatus.Finished;

        await _context.SaveChangesAsync(cancellationToken);

        return lobby;
    }
}