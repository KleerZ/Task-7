namespace Task.Domain;

public class Lobby
{
    public int Id { get; set; }
    public string Status { get; set; }
    public Guid ConnectionId { get; set; }
    public string[]? Field { get; set; } = { "", "", "", "", "", "", "", "", "" };
    public string? PlayerNameStep { get; set; }

    public List<Player> Players { get; set; }
}