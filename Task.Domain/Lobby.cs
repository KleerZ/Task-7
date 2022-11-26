namespace Task.Domain;

public class Lobby
{
    public int Id { get; set; }
    public string Status { get; set; }
    public string ConnectionId { get; set; }
    public string[,] PlayingField { get; set; }
    
    public List<Player> Players { get; set; }
}