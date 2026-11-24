namespace AiDispatcher.Shared.Contracts.Events;

public record AiGenerationResult(
    Guid RequestId,
    long ChatId,
    bool Success,
    string Content,
    string? ErrorMessage);
