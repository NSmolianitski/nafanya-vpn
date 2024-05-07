using NafanyaVPN.Entities.Users;

namespace NafanyaVPN.Entities.PaymentMessages;

public class PaymentMessage
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public long TelegramChatId { get; set; }
    public int TelegramMessageId { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}