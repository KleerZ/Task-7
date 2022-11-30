using MediatR;

namespace Task.Application.CommandsQueries.Lobby.Queries.Get;

public class GetLobbyQuery : IRequest<Domain.Lobby?>
{
    public Guid ConnectionId { get; set; }
}