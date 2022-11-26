using Microsoft.EntityFrameworkCore;
using Task.Domain;

namespace Task.Application.Common.Interfaces;

public interface IApplicationContext
{
    public DbSet<Player> Players { get; set; }
    public DbSet<Lobby> Lobbies { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}