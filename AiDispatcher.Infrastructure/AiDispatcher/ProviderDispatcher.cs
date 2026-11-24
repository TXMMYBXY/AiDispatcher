using AiDispatcher.Domain.Entities;
using AiDispatcher.Domain.Enums;
using AiDispatcher.Infrastructure.Clients;
using AiDispatcher.Infrastructure.Senders;
using AiDispatcher.Shared.Contracts.Events;

namespace AiDispatcher.Infrastructure.AiDispatcher;

public class ProviderDispatcher : IProviderDispatcher
{
    private readonly IReadOnlyDictionary<ModelProvider, IProviderSender> _requestSenders;
    private readonly IWebhookClient _webhookClient;
    
    public ProviderDispatcher(
        IReadOnlyDictionary<ModelProvider, IProviderSender> requestSenders, 
        IWebhookClient webhookClient)
    {
        _requestSenders = requestSenders;
        _webhookClient = webhookClient;
    }

    public async Task DispatchAsync(AiRequest request)
    {
        request.MarkAsProcessing();
        try
        {
            var response = await _requestSenders[request.ModelProviderId].SendRequestAsync(request);
            request.MarkAsCompleted();
            await _webhookClient.SendResultAsync(
                request.WebhookUrl,
                new AiGenerationResult(request.Id, request.UserId, true, response.Content, null));
        }
        catch (Exception ex)
        {
            request.MarkAsFailed();
            await _webhookClient.SendResultAsync(
                request.WebhookUrl,
                new AiGenerationResult(request.Id, request.UserId, false, string.Empty, ex.Message));
        }
    }
}