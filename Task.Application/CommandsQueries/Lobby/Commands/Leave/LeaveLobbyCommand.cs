using MediatR;

namespace Task.Application.CommandsQueries.Lobby.Commands.Leave;

public class LeaveLobbyCommand : IRequest<Domain.Lobby>
{
    public string? PlayerName { get; set; }
    public Guid ConnectionId { get; set; }
}