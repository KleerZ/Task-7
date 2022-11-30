using MediatR;
using Microsoft.EntityFrameworkCore;
using Task.Application.Common.Interfaces;

namespace Task.Application.CommandsQueries.Lobby.Queries.GetStatus;

public class GetLobbyStatusQueryHandler : IRequestHandler<GetLobbyStatusQuery, string>
{
    private readonly IApplicationContext _context;

    public GetLobbyStatusQueryHandler(IApplicationContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(GetLobbyStatusQuery request,
        CancellationToken cancellationToken)
    {
        var lobby = await _context.Lobbies
            .FirstOrDefaultAsync(l => l.ConnectionId == 
                                      Guid.Parse(request.ConnectionId!), cancellationToken);

        return lobby!.Status;
    }
}