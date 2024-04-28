using Microsoft.EntityFrameworkCore;
using NafanyaVPN.Entities.MoneyOperations;
using NafanyaVPN.Entities.Outline;
using NafanyaVPN.Entities.Subscription;
using NafanyaVPN.Entities.Users;
using Type = NafanyaVPN.Entities.MoneyOperations.Type;

namespace NafanyaVPN.Database;

public class NafanyaVPNContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<OutlineKey> OutlineKeys { get; set; } = null!;
    public DbSet<Subscription> Subscriptions { get; set; } = null!;
    
    public DbSet<MoneyOperation> MoneyOperations { get; set; } = null!;
    public DbSet<MoneyOperationType> MoneyOperationTypes { get; set; } = null!;

    public NafanyaVPNContext(DbContextOptions<NafanyaVPNContext> options) : base(options)
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OutlineKey>()
            .HasOne<User>(o => o.User)
            .WithOne(u => u.OutlineKey)
            .HasForeignKey<OutlineKey>(o => o.UserId);

        modelBuilder.Entity<MoneyOperationType>()
            .HasData( 
                new { Id = 1, Type = Type.Deposit },
                new { Id = 2, Type = Type.Withdrawal }
            );
        
        var defaultSubscription = new Subscription
        {
            Id = 1, Name = DatabaseConstants.Default, DailyCostInRoubles = 1
        };
        modelBuilder.Entity<Subscription>().HasData(defaultSubscription);
    }
}