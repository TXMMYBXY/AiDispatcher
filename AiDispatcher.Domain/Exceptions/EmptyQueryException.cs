namespace AiDispatcher.Domain.Exceptions;

public class EmptyQueryException : Exception
{
    public EmptyQueryException(string message) : base(message)
    { }

    public static void ThrowIf(bool condition, string message)
    {
        if (condition)
            throw new EmptyQueryException(message);
    }
}