using AiDispatcher.Domain.Enums;

namespace AiDispatcher.Domain.Entities;

public class AiResponse
{
    public Guid Id { get; private set; }
    public Guid RequestId { get; private set; }
    public string Content { get; private set; }
    public byte[]? ImageData { get; private set; }
    public int TokensUsed { get; private set; }
    public double? Cost { get; private set; }
    public Dictionary<string, string> Metadata { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string? ErrorMessage { get; private set; }

    private AiResponse() { }

    public AiResponse(Guid requestId, string content, int tokensUsed = 0)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Content cannot be empty", nameof(content));

        Id = Guid.NewGuid();
        RequestId = requestId;
        Content = content;
        TokensUsed = tokensUsed;
        Metadata = new Dictionary<string, string>();
        CreatedAt = DateTime.UtcNow;
    }

    public static AiResponse CreateError(Guid requestId, string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(errorMessage))
            throw new ArgumentException("ErrorMessage cannot be empty", nameof(errorMessage));

        var response = new AiResponse
        {
            Id = Guid.NewGuid(),
            RequestId = requestId,
            Content = string.Empty,
            ErrorMessage = errorMessage,
            CreatedAt = DateTime.UtcNow,
            Metadata = new Dictionary<string, string>()
        };

        return response;
    }

    public void SetImageData(byte[] imageData)
    {
        if (imageData == null || imageData.Length == 0)
            throw new ArgumentException("ImageData cannot be empty", nameof(imageData));
        
        ImageData = imageData;
    }

    public void SetCost(double cost)
    {
        if (cost < 0)
            throw new ArgumentException("Cost cannot be negative", nameof(cost));
        
        Cost = cost;
    }

    public void SetMetadata(string key, string value)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Key cannot be empty", nameof(key));
        
        Metadata[key] = value;
    }

    public bool IsSuccessful => string.IsNullOrEmpty(ErrorMessage);
}
