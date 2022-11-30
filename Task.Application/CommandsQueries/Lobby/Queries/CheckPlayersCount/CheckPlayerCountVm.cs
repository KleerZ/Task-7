namespace Task.Application.CommandsQueries.Lobby.Queries.CheckPlayersCount;

public class CheckPlayerCountVm
{
    public Domain.Player? Player { get; set; }
    public int PlayersCount { get; set; }
}