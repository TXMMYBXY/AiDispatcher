namespace AiDispatcher.Domain.Exceptions;

public class GenerationFailedException : Exception
{
    public GenerationFailedException(string message) : base(message)
    { }

    public static void ThrowIf(bool condition, string message)
    {
        if (condition)
            throw new GenerationFailedException(message);
    }
}