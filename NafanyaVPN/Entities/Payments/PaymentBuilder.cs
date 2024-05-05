using NafanyaVPN.Entities.Users;

namespace NafanyaVPN.Entities.Payments;

public class PaymentBuilder
{
    private long _id;
    private DateTime _createdAt;
    private DateTime _updatedAt;
    private User _user;
    private decimal _sum;
    private string _label;
    private PaymentStatus _status;

    public PaymentBuilder WithId(long id)
    {
        _id = id;
        return this;
    }

    public PaymentBuilder WithCreatedAt(DateTime createdAt)
    {
        _createdAt = createdAt;
        return this;
    }

    public PaymentBuilder WithUpdatedAt(DateTime updatedAt)
    {
        _updatedAt = updatedAt;
        return this;
    }

    public PaymentBuilder WithUser(User user)
    {
        _user = user;
        return this;
    }

    public PaymentBuilder WithSum(decimal sum)
    {
        _sum = sum;
        return this;
    }

    public PaymentBuilder WithLabel(string label)
    {
        _label = label;
        return this;
    }

    public PaymentBuilder WithStatus(PaymentStatusType status)
    {
        _status = new PaymentStatus(status);
        return this;
    }

    public Payment Build() => new Payment(_id, _createdAt, _updatedAt, _user, _sum, _label, _status);
}