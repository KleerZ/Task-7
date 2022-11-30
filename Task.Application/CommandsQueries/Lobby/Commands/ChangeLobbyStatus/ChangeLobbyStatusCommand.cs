using MediatR;

namespace Task.Application.CommandsQueries.Lobby.Commands.ChangeLobbyStatus;

public class ChangeLobbyStatusCommand : IRequest
{
    public Guid ConnectionId { get; set; }
    public string? Status { get; set; } 
}