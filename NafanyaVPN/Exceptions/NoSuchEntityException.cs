namespace NafanyaVPN.Exceptions;

public class NoSuchEntityException : Exception
{
    public NoSuchEntityException()
    {
    }

    public NoSuchEntityException(string? message) : base(message)
    {
    }

    public NoSuchEntityException(string? message, Exception? inner) : base(message, inner)
    {
    }
}