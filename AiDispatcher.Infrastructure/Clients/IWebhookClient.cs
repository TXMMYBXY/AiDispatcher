using AiDispatcher.Domain.Entities;

namespace AiDispatcher.Infrastructure.Clients;

public interface IWebhookClient
{
    Task SendResponseAsync(string uri, AiResponse response);
}