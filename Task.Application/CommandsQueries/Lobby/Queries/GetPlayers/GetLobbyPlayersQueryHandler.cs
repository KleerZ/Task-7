using MediatR;
using Microsoft.EntityFrameworkCore;
using Task.Application.Common.Interfaces;

namespace Task.Application.CommandsQueries.Lobby.Queries.GetPlayers;

public class GetLobbyPlayersQueryHandler : IRequestHandler<GetLobbyPlayersQuery, GetLobbyPlayersVm>
{
    private readonly IApplicationContext _context;

    public GetLobbyPlayersQueryHandler(IApplicationContext context)
    {
        _context = context;
    }

    public async Task<GetLobbyPlayersVm> Handle(GetLobbyPlayersQuery request,
        CancellationToken cancellationToken)
    {
        var players = await _context.Lobbies
            .Where(l => l.ConnectionId == request.ConnectionId)
            .Include(l => l.Players)
            .SelectMany(l => l.Players)
            .ToListAsync(cancellationToken);

        return new GetLobbyPlayersVm {Players = players};
    }
}