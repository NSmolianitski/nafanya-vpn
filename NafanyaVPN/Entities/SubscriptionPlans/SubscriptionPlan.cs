namespace NafanyaVPN.Entities.SubscriptionPlans;

/// <summary>
/// Подписочный план, в котором содержится информация о стоимости и пр.
/// </summary>
public class SubscriptionPlan
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Name { get; set; }
    public decimal CostInRoubles { get; set; }
}