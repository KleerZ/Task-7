using MediatR;

namespace Task.Application.CommandsQueries.Player.Queries.GetPlayerStepSymbol;

public class GetPlayerStepSymbolQuery : IRequest<string>
{
    public string? PlayerName { get; set; }
}