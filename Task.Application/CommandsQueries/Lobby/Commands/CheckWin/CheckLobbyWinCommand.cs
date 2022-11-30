using MediatR;

namespace Task.Application.CommandsQueries.Lobby.Commands.CheckWin;

public class CheckLobbyWinCommand : IRequest<string>
{
    public string? ConnectionId { get; set; }
}