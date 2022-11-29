using MediatR;

namespace Task.Application.CommandsQueries.Lobby.Commands.Create;

public class CreateLobbyCommand : IRequest<Guid>
{
    public string? PlayerName { get; set; }
}