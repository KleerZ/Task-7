using MediatR;

namespace Task.Application.CommandsQueries.Lobby.Commands.Restart;

public class RestartLobbyCommand : IRequest
{
    public Guid ConnectionId { get; set; }
}