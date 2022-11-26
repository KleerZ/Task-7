using Microsoft.EntityFrameworkCore;
using Task.Domain;

namespace Task.Persistence;

public class ApplicationContext : DbContext
{
    public DbSet<Player> Players { get; set; }
    public DbSet<Lobby> Lobbies { get; set; }

    public ApplicationContext(DbContextOptions options)
        : base(options)
    {
        Database.EnsureCreated();
    }
}