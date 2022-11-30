using MediatR;

namespace Task.Application.CommandsQueries.Lobby.Commands.Restart;

public class RestartLobbyCommand : IRequest
{
    public string PlayerName { get; set; }
    public Guid ConnectionId { get; set; }
}