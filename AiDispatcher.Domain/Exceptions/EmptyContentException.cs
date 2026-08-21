namespace AiDispatcher.Domain.Exceptions;

public class EmptyContentException : Exception
{
    public EmptyContentException(string message) : base(message)
    { }
    
    public static void ThrowIf(bool condition, string message)
    {
        if (condition)
            throw new EmptyContentException(message);
    }
}