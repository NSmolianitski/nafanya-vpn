using Microsoft.EntityFrameworkCore;
using NafanyaVPN.Entities.Outline;
using NafanyaVPN.Entities.PaymentMessages;
using NafanyaVPN.Entities.Payments;
using NafanyaVPN.Entities.SubscriptionPlans;
using NafanyaVPN.Entities.Subscriptions;
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
    public DbSet<SubscriptionPlan> SubscriptionPlans { get; init; } = null!;
    public DbSet<Payment> Payments { get; init; } = null!;
    public DbSet<Withdraw> Withdraws { get; init; } = null!;
    public DbSet<PaymentStatus> PaymentStatuses { get; init; } = null!;
    public DbSet<PaymentMessage> PaymentMessages { get; init; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Связь User с OutlineKey
        modelBuilder.Entity<User>()
            .HasOne(u => u.OutlineKey)
            .WithOne(o => o.User)
            .HasForeignKey<OutlineKey>(o => o.UserId);

        // Связь User с PaymentMessage
        modelBuilder.Entity<User>()
            .HasOne(u => u.PaymentMessage)
            .WithOne(p => p.User)
            .IsRequired();

        // Связь Subscription с User
        modelBuilder.Entity<Subscription>()
            .HasOne(s => s.User)
            .WithOne(u => u.Subscription)
            .HasForeignKey<Subscription>(s => s.UserId)
            .IsRequired();
        
        // Связь Subscription с SubscriptionPlan
        modelBuilder.Entity<Subscription>()
            .HasOne(s => s.SubscriptionPlan)
            .WithMany()
            .HasForeignKey(s => s.SubscriptionPlanId)
            .IsRequired();

        // Настройка конверсии статуса из БД в Enum
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
        var costInRoubles = decimal.Parse(config[SubscriptionConstants.CostInRoubles]!);
        var defaultSubscriptionPlan = new SubscriptionPlan
        {
            Id = 1,
            CreatedAt = nowDateTime,
            UpdatedAt = nowDateTime,
            Name = DatabaseConstants.Default,
            CostInRoubles = costInRoubles
        };
        modelBuilder.Entity<SubscriptionPlan>().HasData(defaultSubscriptionPlan);
        
#if DEBUG
        var subscription = new SubscriptionBuilder()
            .WithId(1)
            .WithNowCreatedAtUpdatedAt()
            .WithSubscriptionPlanId(1)
            .WithHasExpired(true)
            .WithRenewalDisabled(false)
            .WithEndNotificationsDisabled(false)
            .WithRenewalNotificationsDisabled(false)
            .WithEndNotificationPerformed(false)
            .Build();
        subscription.UserId = 1;
        modelBuilder.Entity<Subscription>().HasData(subscription);
    
        var user = new UserBuilder()
            .WithId(1)
            .WithNowCreatedAtUpdatedAt()
            .WithTelegramChatId(1)
            .WithTelegramUserId(123)
            .WithTelegramUserName("test-telegram-user")
            .WithMoneyInRoubles(0.0m)
            .WithTelegramState(string.Empty)
            .Build();
        modelBuilder.Entity<User>().HasData(user);
# endif
    }
}