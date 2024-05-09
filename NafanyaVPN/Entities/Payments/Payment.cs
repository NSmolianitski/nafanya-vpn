using NafanyaVPN.Entities.Users;

namespace NafanyaVPN.Entities.Payments;

public class Payment
{
    public Payment() {}

    public Payment(
        long id,
        DateTime createdAt,
        DateTime updatedAt,
        User user,
        decimal sum,
        string label,
        PaymentStatusType status)
    {
        Id = id;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        User = user;
        Sum = sum;
        Label = label;
        Status = status;
    }
    
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public decimal Sum { get; set; }
    public string Label { get; set; }
    public PaymentStatusType Status { get; set; }
}