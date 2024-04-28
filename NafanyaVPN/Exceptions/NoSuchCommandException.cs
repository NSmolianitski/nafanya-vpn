namespace NafanyaVPN.Exceptions;

public class NoSuchCommandException : Exception
{
    public NoSuchCommandException()
    {
    }

    public NoSuchCommandException(string message) : base(message)
    {
    }

    public NoSuchCommandException(string message, Exception inner) : base(message, inner)
    {
    }
}