namespace NafanyaVPN.Services.Abstractions;

public interface IPaymentService
{
    Task SendPaymentForm(decimal sum, long userId);
}