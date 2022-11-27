using MediatR;
using Microsoft.EntityFrameworkCore;
using Task.Application.Common.Data;
using Task.Application.Common.Interfaces;

namespace Task.Application.CommandsQueries.Lobby.Commands.Create;

public class CreateLobbyCommandHandler : IRequestHandler<CreateLobbyCommand, Guid>
{
    private readonly IApplicationContext _context;

    public CreateLobbyCommandHandler(IApplicationContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateLobbyCommand request,
        CancellationToken cancellationToken)
    {
        var player = await _context.Players
            .FirstOrDefaultAsync(p => p.Name == request.PlayerName, cancellationToken);

        if (player is null)
            throw new NullReferenceException($"The player named '{request.PlayerName}' is not exists");

        var lobby = new Domain.Lobby
        {
            Status = LobbyStatus.WaitingForPlayers,
            ConnectionId = Guid.NewGuid(),
            Players = new List<Domain.Player> { player }
        };

        await _context.Lobbies.AddAsync(lobby, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return lobby.ConnectionId;
    }
}