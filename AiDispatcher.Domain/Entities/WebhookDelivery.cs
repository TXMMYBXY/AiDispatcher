namespace AiDispatcher.Domain.Entities;

public class WebhookDelivery
{
    public Guid Id { get; private set; }
    public Guid ResponseId { get; private set; }
    public string WebhookUrl { get; private set; }
    public int? HttpStatusCode { get; private set; }
    public string? ResponseContent { get; private set; }
    public bool IsSuccessful { get; private set; }
    public int AttemptNumber { get; private set; }
    public DateTime DeliveredAt { get; private set; }
    public string? ErrorMessage { get; private set; }

    public WebhookDelivery(Guid responseId, string webhookUrl, int attemptNumber = 1)
    {
        if (string.IsNullOrWhiteSpace(webhookUrl))
            throw new ArgumentException("WebhookUrl cannot be empty", nameof(webhookUrl));
        if (attemptNumber <= 0)
            throw new ArgumentException("AttemptNumber must be greater than 0", nameof(attemptNumber));

        Id = Guid.NewGuid();
        ResponseId = responseId;
        WebhookUrl = webhookUrl;
        AttemptNumber = attemptNumber;
        DeliveredAt = DateTime.UtcNow;
        IsSuccessful = false;
    }

    public void MarkAsSuccessful(int statusCode, string? responseContent = null)
    {
        IsSuccessful = statusCode >= 200 && statusCode < 300;
        HttpStatusCode = statusCode;
        ResponseContent = responseContent;
    }

    public void MarkAsFailed(string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(errorMessage))
            throw new ArgumentException("ErrorMessage cannot be empty", nameof(errorMessage));
        
        IsSuccessful = false;
        ErrorMessage = errorMessage;
    }
}
