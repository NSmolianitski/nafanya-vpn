using Microsoft.EntityFrameworkCore;
using NafanyaVPN.Entities.Outline;
using NafanyaVPN.Entities.Payments;
using NafanyaVPN.Entities.Subscription;
using NafanyaVPN.Entities.Users;
using NafanyaVPN.Entities.Withdraws;
using NafanyaVPN.Utils;

namespace NafanyaVPN.Database;

public class NafanyaVPNContext : DbContext
{
    public DbSet<User> Users { get; init; } = null!;
    public DbSet<OutlineKey> OutlineKeys { get; init; } = null!;
    public DbSet<Subscription> Subscriptions { get; init; } = null!;
    public DbSet<Payment> Payments { get; init; } = null!;
    public DbSet<Withdraw> Withdraws { get; init; } = null!;
    public DbSet<PaymentStatus> PaymentStatuses { get; init; } = null!;

    public NafanyaVPNContext(DbContextOptions<NafanyaVPNContext> options) : base(options)
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasOne(u => u.OutlineKey)
            .WithOne(o => o.User)
            .HasForeignKey<OutlineKey>(o => o.UserId);

        var nowDateTime = DateTimeUtils.GetMoscowTime();
        modelBuilder.Entity<PaymentStatus>()
            .HasData(
                new
                {
                    Id = 1, CreatedAt = nowDateTime, UpdatedAt = nowDateTime, 
                    Type = PaymentStatusType.Finished, Name = PaymentStatusType.Finished.ToString()
                },
                new
                {
                    Id = 2, CreatedAt = nowDateTime, UpdatedAt = nowDateTime, 
                    Type = PaymentStatusType.Waiting, Name = PaymentStatusType.Waiting.ToString()
                },
                new
                {
                    Id = 3, CreatedAt = nowDateTime, UpdatedAt = nowDateTime, 
                    Type = PaymentStatusType.Canceled, Name = PaymentStatusType.Canceled.ToString()
                }
            );

        var defaultSubscription = new Subscription
        {
            Id = 1,
            CreatedAt = nowDateTime,
            UpdatedAt = nowDateTime,
            Name = DatabaseConstants.Default,
            DailyCostInRoubles = 1
        };
        modelBuilder.Entity<Subscription>().HasData(defaultSubscription);
    }
}