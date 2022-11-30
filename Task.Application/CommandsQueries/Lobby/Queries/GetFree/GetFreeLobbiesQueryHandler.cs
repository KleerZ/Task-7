using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Task.Application.Common.Data;
using Task.Application.Common.Interfaces;

namespace Task.Application.CommandsQueries.Lobby.Queries.GetFree;

public class GetFreeLobbiesQueryHandler : IRequestHandler<GetFreeLobbiesQuery, LobbiesVm>
{
    private readonly IApplicationContext _context;
    private readonly IMapper _mapper;

    public GetFreeLobbiesQueryHandler(IApplicationContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<LobbiesVm> Handle(GetFreeLobbiesQuery request,
        CancellationToken cancellationToken)
    {
        var lobbies = await _context.Lobbies
            .Include(l => l.Players)
            .Where(l => l.Status == LobbyStatus.WaitingForPlayers)
            .ProjectTo<LobbyDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new LobbiesVm { Lobbies = lobbies };
    }
}