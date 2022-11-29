using Task.Domain;

namespace Task.Mvc.Models;

public class LobbyViewModel
{
    public string? PlayerName { get; set; }
    public Guid? ConnectionId { get; set; }
    public List<Lobby>? Lobbies { get; set; }
}