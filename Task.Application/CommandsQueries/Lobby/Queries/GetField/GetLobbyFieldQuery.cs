using MediatR;

namespace Task.Application.CommandsQueries.Lobby.Queries.GetField;

public class GetLobbyFieldQuery : IRequest<string[]>
{
    public string ConnectionId { get; set; }
}