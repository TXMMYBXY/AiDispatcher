namespace AiDispatcher.Domain.Entities;

public class AiContext
{
    public Guid Id { get; private set; }
    public string RawQuestion { get; private set; }
    public byte[]? ImageBytes { get; private set; }
    public Dictionary<string, object> AdditionalContext { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public AiContext(string question, byte[]? image = null)
    {
        if (string.IsNullOrWhiteSpace(question) && (image == null || image.Length == 0))
            throw new ArgumentException("Either question or image must be provided");

        Id = Guid.NewGuid();
        RawQuestion = question?.Trim() ?? string.Empty;
        ImageBytes = image;
        AdditionalContext = new Dictionary<string, object>();
        CreatedAt = DateTime.UtcNow;
    }

    public bool IsValid() => !string.IsNullOrEmpty(RawQuestion) || ImageBytes != null;
    public bool HasImage() => ImageBytes != null && ImageBytes.Length > 0;
    public bool HasQuestion() => !string.IsNullOrEmpty(RawQuestion);

    public void AddContext(string key, object value)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Key cannot be empty", nameof(key));
        
        AdditionalContext[key] = value ?? throw new ArgumentNullException(nameof(value));
    }

    public object? GetContext(string key)
    {
        return AdditionalContext.TryGetValue(key, out var value) ? value : null;
    }
}