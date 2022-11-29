using MediatR;

namespace Task.Application.CommandsQueries.Lobby.Queries.GetStatus;

public class GetLobbyStatusQuery : IRequest<string>
{
    public string? ConnectionId { get; set; }
}