using System.Security.Claims;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Task.Application.Common.Interfaces;

namespace Task.Application.CommandsQueries.Player.Commands.Login;

public class PlayerLoginCommandHandler : IRequestHandler<PlayerLoginCommand, ClaimsIdentity>
{
    private readonly IApplicationContext _context;

    public PlayerLoginCommandHandler(IApplicationContext context)
    {
        _context = context;
    }
    
    public async Task<ClaimsIdentity> Handle(PlayerLoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _context.Players
            .FirstOrDefaultAsync(u => u.Name == request.Name, cancellationToken);

        if (user is null)
        {
            user = new Domain.Player { Name = request.Name };
            
            await _context.Players.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Name),
        };

        var claimsIdentity = new ClaimsIdentity(claims, "ApplicationCookie", 
            ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

        return claimsIdentity;
    }
}