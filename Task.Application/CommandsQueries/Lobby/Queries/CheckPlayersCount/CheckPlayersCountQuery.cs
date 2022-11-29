using MediatR;

namespace Task.Application.CommandsQueries.Lobby.Queries.CheckPlayersCount;

public class CheckPlayersCountQuery : IRequest<CheckPlayerCountVm>
{
    public string PlayerName { get; set; }
    public string ConnectionId { get; set; }
}