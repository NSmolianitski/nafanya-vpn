using NafanyaVPN.Entities.Users;

namespace NafanyaVPN.Entities.Payments;

public class Payment
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime DateTime { get; set; }
    public User User { get; set; }
    public decimal Sum { get; set; }
    public string Label { get; set; }
    public PaymentStatusType Status { get; set; }
}