namespace Task.Domain;

public class Player
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public List<Lobby> Lobbies { get; set; }
}