using MediatR;
using Microsoft.EntityFrameworkCore;
using Task.Application.Common.Interfaces;

namespace Task.Application.CommandsQueries.Lobby.Queries.CheckPlayersCount;

public class CheckPlayersCountQueryHandler : IRequestHandler<CheckPlayersCountQuery, CheckPlayerCountVm>
{
    private readonly IApplicationContext _context;

    public CheckPlayersCountQueryHandler(IApplicationContext context)
    {
        _context = context;
    }

    public async Task<CheckPlayerCountVm> Handle(CheckPlayersCountQuery request,
        CancellationToken cancellationToken)
    {
        var lobby = await _context.Lobbies
            .Include(p => p.Players)
            .FirstOrDefaultAsync(l => 
                l.ConnectionId.ToString() == request.ConnectionId, cancellationToken);

        if (lobby is null)
            throw new NullReferenceException("The lobby does not exist");

        var playersCount = lobby.Players.Count;

        var secondPlayer = lobby.Players.FirstOrDefault(p => p.Name != request.PlayerName);

        return new CheckPlayerCountVm
        {
            Player = secondPlayer,
            PlayersCount = playersCount
        };
    }
}