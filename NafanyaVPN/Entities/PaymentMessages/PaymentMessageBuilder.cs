using NafanyaVPN.Entities.Users;

namespace NafanyaVPN.Entities.Payments;

public class PaymentMessageBuilder
{
    private int _id;
    private DateTime _createdAt;
    private DateTime _updatedAt;
    private long _telegramChatId;
    private int _telegramMessageId;
    private User _user;

    public PaymentMessageBuilder WithId(int id)
    {
        _id = id;
        return this;
    }
    
    public PaymentMessageBuilder WithCreatedAt(DateTime createdAt)
    {
        _createdAt = createdAt;
        return this;
    }

    public PaymentMessageBuilder WithUpdatedAt(DateTime updatedAt)
    {
        _updatedAt = updatedAt;
        return this;
    }
    
    public PaymentMessageBuilder WithTelegramChatId(long chatId)
    {
        _telegramChatId = chatId;
        return this;
    }

    public PaymentMessageBuilder WithTelegramMessageId(int messageId)
    {
        _telegramMessageId = messageId;
        return this;
    }
    
    public PaymentMessageBuilder WithUser(User user)
    {
        _user = user;
        return this;
    }

    public PaymentMessage Build() => new PaymentMessage { 
        Id = _id,
        CreatedAt = _createdAt,
        UpdatedAt = _updatedAt,
        TelegramChatId = _telegramChatId, 
        TelegramMessageId = _telegramMessageId, 
        User = _user
    };
}