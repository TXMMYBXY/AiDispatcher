using AiDispatcher.Domain.Enums;

namespace AiDispatcher.Domain.Entities;

public class AiRequest
{
    public Guid Id { get; private set; }
    public long UserId { get; private set; }
    public ModelProvider ModelProviderId { get; private set; }
    public string Content { get; private set; }
    public byte[]? ImageData { get; private set; }
    public RequestStatus Status { get; private set; }
    public ResponseChannel ResponseChannel { get; private set; }
    public string WebhookUrl { get; private set; }
    public Dictionary<string, string> Metadata { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ProcessedAt { get; private set; }
    public int RetryCount { get; private set; }
    public int MaxRetries { get; private set; }

    public AiRequest(
        long userId,
        ModelProvider modelProviderId,
        string content,
        ResponseChannel responseChannel,
        string webhookUrl,
        byte[]? imageData = null)
    {
        if (string.IsNullOrWhiteSpace(content) && (imageData == null || imageData.Length == 0))
            throw new ArgumentException("Either content or imageData must be provided");

        if (responseChannel == ResponseChannel.Webhook && string.IsNullOrWhiteSpace(webhookUrl))
            throw new ArgumentException("WebhookUrl is required when ResponseChannel is Webhook", nameof(webhookUrl));

        Id = Guid.NewGuid();
        ModelProviderId = modelProviderId;
        UserId = userId;
        Content = content ?? string.Empty;
        ImageData = imageData;
        Status = RequestStatus.Pending;
        ResponseChannel = responseChannel;
        WebhookUrl = webhookUrl;
        Metadata = new Dictionary<string, string>();
        CreatedAt = DateTime.UtcNow;
        RetryCount = 0;
        MaxRetries = 3;
    }

    public void MarkAsProcessing()
    {
        if (Status != RequestStatus.Pending)
            throw new InvalidOperationException($"Cannot process request in {Status} status");
        
        Status = RequestStatus.Processing;
    }

    public void MarkAsCompleted()
    {
        if (Status != RequestStatus.Processing)
            throw new InvalidOperationException($"Cannot complete request in {Status} status");
        
        Status = RequestStatus.Completed;
        ProcessedAt = DateTime.UtcNow;
    }

    public void MarkAsFailed()
    {
        Status = RequestStatus.Failed;
        ProcessedAt = DateTime.UtcNow;
    }

    public void MarkAsTimeout()
    {
        Status = RequestStatus.Timeout;
        ProcessedAt = DateTime.UtcNow;
    }

    public void IncrementRetry()
    {
        if (RetryCount >= MaxRetries)
            throw new InvalidOperationException($"Max retries ({MaxRetries}) exceeded");
        
        RetryCount++;
        Status = RequestStatus.Pending;
    }

    public bool CanRetry() => RetryCount < MaxRetries;

    public void SetMetadata(string key, string value)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Key cannot be empty", nameof(key));
        
        Metadata[key] = value;
    }
}
