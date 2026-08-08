namespace AiDispatcher.Domain.Entities;

public class AiContext
{
    public string RawQuestion { get; private set; }
    public byte[]? ImageBytes { get; private set; }

    public AiContext(string question, byte[]? image)
    {
        RawQuestion = question?.Trim() ?? string.Empty;
        ImageBytes = image;
    }

    public bool IsValid() => !string.IsNullOrEmpty(RawQuestion) || ImageBytes != null;

    public bool HasImage() => ImageBytes != null && ImageBytes.Length > 0;
}