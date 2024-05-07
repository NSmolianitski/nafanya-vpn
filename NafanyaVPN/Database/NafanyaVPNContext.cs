using Microsoft.EntityFrameworkCore;
using NafanyaVPN.Entities.Outline;
using NafanyaVPN.Entities.PaymentMessages;
using NafanyaVPN.Entities.Payments;
using NafanyaVPN.Entities.Subscription;
using NafanyaVPN.Entities.Users;
using NafanyaVPN.Entities.Withdraws;
using NafanyaVPN.Utils;

namespace NafanyaVPN.Database;

public class NafanyaVPNContext(
    DbContextOptions<NafanyaVPNContext> options, 
    IConfiguration configuration) 
    : DbContext(options)
{
    public DbSet<User> Users { get; init; } = null!;
    public DbSet<OutlineKey> OutlineKeys { get; init; } = null!;
    public DbSet<Subscription> Subscriptions { get; init; } = null!;
    public DbSet<Payment> Payments { get; init; } = null!;
    public DbSet<Withdraw> Withdraws { get; init; } = null!;
    public DbSet<PaymentStatus> PaymentStatuses { get; init; } = null!;
    public DbSet<PaymentMessage> PaymentMessages { get; init; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasOne(u => u.OutlineKey)
            .WithOne(o => o.User)
            .HasForeignKey<OutlineKey>(o => o.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasOne(u => u.PaymentMessage)
            .WithOne(p => p.User)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Payment>()
            .Property(p => p.Status)
            .HasColumnName("StatusId")
            .HasConversion(
                p => p.ToString(),
                p => (PaymentStatusType) Enum.Parse(typeof(PaymentStatusType), p)
            );
        
        AddInitialData(modelBuilder);
    }

    private void AddInitialData(ModelBuilder modelBuilder)
    {
        var nowDateTime = DateTimeUtils.GetMoscowNowTime();
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

        var config = configuration.GetRequiredSection(SubscriptionConstants.Subscription);
        var costInRoubles = decimal.Parse(config[SubscriptionConstants.SubscriptionCostInRoubles]!);
        var defaultSubscription = new Subscription
        {
            Id = 1,
            CreatedAt = nowDateTime,
            UpdatedAt = nowDateTime,
            Name = DatabaseConstants.Default,
            CostInRoubles = costInRoubles
        };
        modelBuilder.Entity<Subscription>().HasData(defaultSubscription);
    }
}