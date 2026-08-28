using System.Text.Json.Serialization;

namespace AiDispatcher.Infrastructure.Clients;

public class ErrorResponse
{
    [JsonPropertyName("Message")]
    public string Message { get; set; } = string.Empty;
}