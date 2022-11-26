using Microsoft.EntityFrameworkCore;
using Task.Application.Common.Interfaces;
using Task.Domain;

namespace Task.Persistence;

public class ApplicationContext : DbContext, IApplicationContext
{
    public DbSet<Player> Players { get; set; }
    public DbSet<Lobby> Lobbies { get; set; }

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
}