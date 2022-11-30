namespace Task.Application.CommandsQueries.Lobby.Queries.GetPlayers;

public class GetLobbyPlayersVm
{
    public IEnumerable<Domain.Player>? Players { get; set; }
}