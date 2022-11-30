using MediatR;

namespace Task.Application.CommandsQueries.Lobby.Commands.Step;

public class LobbyStepCommand : IRequest<string[]>
{
    public string? ConnectionId { get; set; }
    public string? Symbol { get; set; }
    public int CellIndex { get; set; }
}