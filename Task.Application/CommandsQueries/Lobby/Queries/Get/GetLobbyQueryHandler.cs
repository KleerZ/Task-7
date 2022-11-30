using MediatR;
using Microsoft.EntityFrameworkCore;
using Task.Application.Common.Data;
using Task.Application.Common.Interfaces;

namespace Task.Application.CommandsQueries.Lobby.Queries.Get;

public class GetLobbyQueryHandler : IRequestHandler<GetLobbyQuery, Domain.Lobby?>
{
    private readonly IApplicationContext _context;

    public GetLobbyQueryHandler(IApplicationContext context)
    {
        _context = context;
    }

    public async Task<Domain.Lobby?> Handle(GetLobbyQuery request,
        CancellationToken cancellationToken)
    {
        var lobby = await _context.Lobbies
            .FirstOrDefaultAsync(l => l.ConnectionId == 
                request.ConnectionId && l.Status == LobbyStatus.WaitingForPlayers, 
                cancellationToken);

        return lobby;
    }
}