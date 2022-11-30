using MediatR;

namespace Task.Application.CommandsQueries.Lobby.Queries.GetPlayers;

public class GetLobbyPlayersQuery : IRequest<GetLobbyPlayersVm>
{
    public Guid ConnectionId { get; set; }
}