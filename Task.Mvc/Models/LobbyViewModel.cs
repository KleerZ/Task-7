using Task.Application.CommandsQueries.Lobby.Queries.GetFree;
using Task.Domain;

namespace Task.Mvc.Models;

public class LobbyViewModel
{
    public string? PlayerName { get; set; }
    public Guid? ConnectionId { get; set; }
    public List<LobbyDto>? Lobbies { get; set; }
}