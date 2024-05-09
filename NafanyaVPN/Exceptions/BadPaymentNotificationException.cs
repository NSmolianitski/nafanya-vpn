namespace NafanyaVPN.Exceptions;

public class BadPaymentNotificationException : Exception
{
    public BadPaymentNotificationException()
    {
    }

    public BadPaymentNotificationException(string message) : base(message)
    {
    }

    public BadPaymentNotificationException(string message, Exception inner) : base(message, inner)
    {
    }
}