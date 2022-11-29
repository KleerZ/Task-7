using MediatR;
using Microsoft.EntityFrameworkCore;
using Task.Application.Common.Interfaces;

namespace Task.Application.CommandsQueries.Lobby.Queries.GetPlayerNameStep;

public class GetPlayerNameStepQueryHandler : IRequestHandler<GetPlayerNameStepQuery, string?>
{
    private readonly IApplicationContext _context;

    public GetPlayerNameStepQueryHandler(IApplicationContext context)
    {
        _context = context;
    }

    public async Task<string?> Handle(GetPlayerNameStepQuery request,
        CancellationToken cancellationToken)
    {
        var playerName = (await _context.Lobbies
            .FirstOrDefaultAsync(l => l.ConnectionId == 
                                      request.ConnectionId, cancellationToken))?.PlayerNameStep;

        return playerName;
    }
}