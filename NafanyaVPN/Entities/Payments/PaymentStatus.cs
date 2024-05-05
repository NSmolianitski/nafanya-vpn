namespace NafanyaVPN.Entities.Payments;

public class PaymentStatus
{
    public PaymentStatus() {}
    
    public PaymentStatus(PaymentStatusType type)
    {
        Id = (int) type;
        Name = type.ToString();
    }
    
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Name { get; set; }
}