using AiDispatcher.Domain.Enums;

namespace AiDispatcher.Domain.Entities;

public class AiAgent
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public AgentType Type { get; private set; }
    public string Endpoint { get; private set; }
    public string ApiKey { get; private set; }
    public bool IsActive { get; private set; }
    public int MaxConcurrentRequests { get; private set; }
    public TimeSpan RequestTimeout { get; private set; }
    public Dictionary<string, string> Metadata { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public AiAgent(
        string name,
        AgentType type,
        string endpoint,
        string apiKey,
        int maxConcurrentRequests = 10,
        TimeSpan? requestTimeout = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));
        if (string.IsNullOrWhiteSpace(endpoint))
            throw new ArgumentException("Endpoint cannot be empty", nameof(endpoint));
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new ArgumentException("ApiKey cannot be empty", nameof(apiKey));
        if (maxConcurrentRequests <= 0)
            throw new ArgumentException("MaxConcurrentRequests must be greater than 0", nameof(maxConcurrentRequests));

        Id = Guid.NewGuid();
        Name = name;
        Type = type;
        Endpoint = endpoint;
        ApiKey = apiKey;
        MaxConcurrentRequests = maxConcurrentRequests;
        RequestTimeout = requestTimeout ?? TimeSpan.FromSeconds(30);
        IsActive = true;
        Metadata = new Dictionary<string, string>();
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateEndpoint(string newEndpoint)
    {
        if (string.IsNullOrWhiteSpace(newEndpoint))
            throw new ArgumentException("Endpoint cannot be empty", nameof(newEndpoint));
        
        Endpoint = newEndpoint;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateApiKey(string newApiKey)
    {
        if (string.IsNullOrWhiteSpace(newApiKey))
            throw new ArgumentException("ApiKey cannot be empty", nameof(newApiKey));
        
        ApiKey = newApiKey;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetMetadata(string key, string value)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Key cannot be empty", nameof(key));
        
        Metadata[key] = value;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
