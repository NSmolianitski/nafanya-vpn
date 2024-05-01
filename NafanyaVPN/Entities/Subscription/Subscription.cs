namespace NafanyaVPN.Entities.Subscription;

public class Subscription
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Name { get; set; }
    public decimal DailyCostInRoubles { get; set; }
    public DateTime NextUpdateTime { get; set; }
}