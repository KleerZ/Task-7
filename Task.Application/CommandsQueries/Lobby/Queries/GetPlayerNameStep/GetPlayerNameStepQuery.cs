using MediatR;

namespace Task.Application.CommandsQueries.Lobby.Queries.GetPlayerNameStep;

public class GetPlayerNameStepQuery : IRequest<string?>
{
    public Guid ConnectionId { get; set; }
}