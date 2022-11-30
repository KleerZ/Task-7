using System.Security.Claims;
using MediatR;

namespace Task.Application.CommandsQueries.Player.Commands.Login;

public class PlayerLoginCommand : IRequest<ClaimsIdentity>
{
    public string Name { get; set; }
}