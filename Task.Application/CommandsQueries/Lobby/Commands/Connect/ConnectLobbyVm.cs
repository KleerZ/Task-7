using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Task.Application.CommandsQueries.Lobby.Commands.Connect;

public class ConnectLobbyVm
{
    public string? PlayerName { get; set; }
    public Guid ConnectionId { get; set; }
    public ModelStateDictionary ModelState { get; set; }
}