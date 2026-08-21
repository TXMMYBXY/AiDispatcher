namespace AiDispatcher.Domain.Exceptions;

public class InvalidRequestException : Exception
{
    public InvalidRequestException(string message) : base(message)
    { }

    public static void ThrowIf(bool condition, string message)
    {
        if (condition)
            throw new InvalidRequestException(message);
    }
}
