using AiDispatcher.Domain.Enums;

namespace AiDispatcher.Domain.Entities;

public class UserIntent
{
    public Guid Id { get; private set; }
    public IntentType IntentType { get; private set; }
    public string Description { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public UserIntent() { }

    public UserIntent(string description, IntentType intentType)
    {
        Id = Guid.NewGuid();
        Description = description ?? throw new ArgumentNullException(nameof(description));
        IntentType = intentType;
        CreatedAt = DateTime.UtcNow;
    }

    public static UserIntent FromContent(object content, string description = "")
    {
        var intentType = content is byte[] ? IntentType.Image : IntentType.Reference;
        return new UserIntent(description, intentType);
    }
}