using MediatR;
using Microsoft.EntityFrameworkCore;
using Task.Application.Common.Data;
using Task.Application.Common.Game;
using Task.Application.Common.Interfaces;

namespace Task.Application.CommandsQueries.Lobby.Commands.CheckWin;

public class CheckLobbyWinCommandHandler : IRequestHandler<CheckLobbyWinCommand, string>
{
    private readonly IApplicationContext _context;

    public CheckLobbyWinCommandHandler(IApplicationContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(CheckLobbyWinCommand request,
        CancellationToken cancellationToken)
    {
        var ticTacToe = new TicTacToe();
        
        var lobby = await _context.Lobbies
            .FirstOrDefaultAsync(l => l.ConnectionId == 
                                      Guid.Parse(request.ConnectionId!), cancellationToken);
        
        var result = ticTacToe.GetWinnerSymbol(lobby!.Field!);

        if (result is null)
            return string.Empty;
        
        lobby.Status = LobbyStatus.WaitingForPlayers;
        await _context.SaveChangesAsync(cancellationToken);

        if (result == Winners.Draw)
            return result;

        var player = _context.Lobbies
            .Include(l => l.Players)
            .SelectMany(l => l.Players)
            .FirstOrDefault(p => p.StepSymbol == result);

        return player.Name;
    }
}