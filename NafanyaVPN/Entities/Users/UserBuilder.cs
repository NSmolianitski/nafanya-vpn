using NafanyaVPN.Entities.Outline;
using NafanyaVPN.Entities.PaymentMessages;
using NafanyaVPN.Entities.Subscriptions;
using NafanyaVPN.Utils;

namespace NafanyaVPN.Entities.Users;

public class UserBuilder
{
    private int _id;
    private DateTime _createdAt;
    private DateTime _updatedAt;
    private string _telegramUserName;
    private long _telegramUserId;
    private long _telegramChatId;
    private decimal _moneyInRoubles;
    private string _telegramState = string.Empty;
    private PaymentMessage? _paymentMessage;
    private OutlineKey? _outlineKey;
    private Subscription _subscription;
    
    public UserBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public UserBuilder WithCreatedAt(DateTime createdAt)
    {
        _createdAt = createdAt;
        return this;
    }
    
    public UserBuilder WithNowCreatedAt()
    {
        _createdAt = DateTimeUtils.GetMoscowNowTime();
        return this;
    }
    
    public UserBuilder WithNowCreatedAtUpdatedAt()
    {
        var nowTime = DateTimeUtils.GetMoscowNowTime();
        _createdAt = nowTime;
        _updatedAt = nowTime;
        return this;
    }

    public UserBuilder WithUpdatedAt(DateTime updatedAt)
    {
        _updatedAt = updatedAt;
        return this;
    }
    
    public UserBuilder WithNowUpdatedAt()
    {
        _updatedAt = DateTimeUtils.GetMoscowNowTime();
        return this;
    }

    public UserBuilder WithTelegramUserName(string telegramUserName)
    {
        _telegramUserName = telegramUserName;
        return this;
    }
    
    public UserBuilder WithTelegramUserId(long telegramUserId)
    {
        _telegramUserId = telegramUserId;
        return this;
    }
    
    public UserBuilder WithTelegramChatId(long telegramChatId)
    {
        _telegramChatId = telegramChatId;
        return this;
    }
    
    public UserBuilder WithMoneyInRoubles(decimal moneyInRoubles)
    {
        _moneyInRoubles = moneyInRoubles;
        return this;
    }
    
    public UserBuilder WithTelegramState(string telegramState)
    {
        _telegramState = telegramState;
        return this;
    }
    
    public UserBuilder WithPaymentMessage(PaymentMessage paymentMessage)
    {
        _paymentMessage = paymentMessage;
        return this;
    }
    
    public UserBuilder WithOutlineKey(OutlineKey outlineKey)
    {
        _outlineKey = outlineKey;
        return this;
    }
    
    public UserBuilder WithSubscription(Subscription subscription)
    {
        _subscription = subscription;
        return this;
    }

    public User Build() => new User(
        _id,
        _createdAt,
        _updatedAt,
        _telegramUserName,
        _telegramUserId,
        _telegramChatId,
        _moneyInRoubles,
        _telegramState,
        _paymentMessage,
        _outlineKey,
        _subscription
    );
}