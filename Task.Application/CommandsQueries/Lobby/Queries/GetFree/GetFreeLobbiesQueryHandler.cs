using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Task.Application.Common.Data;
using Task.Application.Common.Interfaces;

namespace Task.Application.CommandsQueries.Lobby.Queries.GetFree;

public class GetFreeLobbiesQueryHandler : IRequestHandler<GetFreeLobbiesQuery, LobbiesVm>
{
    private readonly IApplicationContext _context;

    public GetFreeLobbiesQueryHandler(IApplicationContext context)
    {
        _context = context;
    }

    public async Task<LobbiesVm> Handle(GetFreeLobbiesQuery request,
        CancellationToken cancellationToken)
    {
        var lobbies = await _context.Lobbies
            .Include(l => l.Players)
            .Where(l => l.Status == LobbyStatus.WaitingForPlayers)
            .ToListAsync(cancellationToken);

        return new LobbiesVm { Lobbies = lobbies };
    }
}