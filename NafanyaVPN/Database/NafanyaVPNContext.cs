using Microsoft.EntityFrameworkCore;
using NafanyaVPN.Entities.MoneyOperations;
using NafanyaVPN.Entities.Outline;
using NafanyaVPN.Entities.Subscription;
using NafanyaVPN.Entities.Users;

namespace NafanyaVPN.Database;

public class NafanyaVPNContext : DbContext
{
    public DbSet<User> Users { get; init; } = null!;
    public DbSet<OutlineKey> OutlineKeys { get; init; } = null!;
    public DbSet<Subscription> Subscriptions { get; init; } = null!;
    public DbSet<MoneyOperation> MoneyOperations { get; init; } = null!;
    public DbSet<MoneyOperationType> MoneyOperationTypes { get; init; } = null!;

    public NafanyaVPNContext(DbContextOptions<NafanyaVPNContext> options) : base(options)
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
        // modelBuilder.Entity<OutlineKey>()
        //     .HasOne<User>(o => o.User)
        //     .WithOne(u => u.OutlineKey)
        //     .HasForeignKey<OutlineKey>(o => o.UserId);
        
        // modelBuilder.Entity<MoneyOperationType>()
        //     .HasData( 
        //         new { Id = 1, Type = OperationType.Deposit },
        //         new { Id = 2, Type = OperationType.Withdrawal }
        //     );
        
        // var defaultSubscription = new Subscription
        // {
        //     Id = 1, Name = DatabaseConstants.Default, DailyCostInRoubles = 1
        // };
        // modelBuilder.Entity<Subscription>().HasData(defaultSubscription);
    // }
}