using MediatR;
using Microsoft.EntityFrameworkCore;
using Task.Application.Common.Interfaces;

namespace Task.Application.CommandsQueries.Lobby.Queries.GetField;

public class GetLobbyFieldQueryHandler : IRequestHandler<GetLobbyFieldQuery, string[]>
{
    private readonly IApplicationContext _context;

    public GetLobbyFieldQueryHandler(IApplicationContext context)
    {
        _context = context;
    }

    public async Task<string[]> Handle(GetLobbyFieldQuery request,
        CancellationToken cancellationToken)
    {
        var lobby = await _context.Lobbies
            .FirstOrDefaultAsync(l => l.ConnectionId == Guid.Parse(request.ConnectionId),
                cancellationToken);

        if (lobby is null)
            throw new NullReferenceException("The lobby does not exist");

        return lobby.Field!;
    }
}