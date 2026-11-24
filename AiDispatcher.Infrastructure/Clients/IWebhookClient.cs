using AiDispatcher.Domain.Entities;
using AiDispatcher.Shared.Contracts.Events;

namespace AiDispatcher.Infrastructure.Clients;

public interface IWebhookClient
{
    Task SendResponseAsync(string uri, AiResponse response);
    Task SendResultAsync(string uri, AiGenerationResult result);
}