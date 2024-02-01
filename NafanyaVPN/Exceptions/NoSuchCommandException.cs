namespace NafanyaVPN.Exceptions;

public class NoSuchCommandException : Exception
{
    public override string Message => "No such command!";

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