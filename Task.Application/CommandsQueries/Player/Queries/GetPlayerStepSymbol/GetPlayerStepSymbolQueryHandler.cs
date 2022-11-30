using MediatR;
using Microsoft.EntityFrameworkCore;
using Task.Application.Common.Interfaces;

namespace Task.Application.CommandsQueries.Player.Queries.GetPlayerStepSymbol;

public class GetPlayerStepSymbolQueryHandler : IRequestHandler<GetPlayerStepSymbolQuery, string?>
{
    private readonly IApplicationContext _context;

    public GetPlayerStepSymbolQueryHandler(IApplicationContext context)
    {
        _context = context;
    }

    public async Task<string?> Handle(GetPlayerStepSymbolQuery request,
        CancellationToken cancellationToken)
    {
        var player = await _context.Players
            .FirstOrDefaultAsync(p => p.Name == request.PlayerName, cancellationToken);

        if (player is null)
            throw new NullReferenceException("The player does not exist");
        
        return player.StepSymbol;
    }
}