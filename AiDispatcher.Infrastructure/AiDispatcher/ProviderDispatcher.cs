using AiDispatcher.Domain.Entities;
using AiDispatcher.Domain.Enums;
using AiDispatcher.Infrastructure.Clients;
using AiDispatcher.Infrastructure.Senders;

namespace AiDispatcher.Infrastructure.AiDispatcher;

public class ProviderDispatcher : IProviderDispatcher
{
    private readonly IReadOnlyDictionary<ModelProvider, IRequestSender> _requestSenders;
    private readonly IWebhookClient _webhookClient;
    
    public ProviderDispatcher(
        IReadOnlyDictionary<ModelProvider, IRequestSender> requestSenders, 
        IWebhookClient webhookClient)
    {
        _requestSenders = requestSenders;
        _webhookClient = webhookClient;
    }

    public async Task DispatchAsync(AiRequest request)
    {
        request.MarkAsProcessing();
        
        var response = await _requestSenders[request.ModelProviderId].SendRequestAsync(request);
        
        request.MarkAsCompleted();
        
        await _webhookClient.SendResponseAsync(request.WebhookUrl, response);
    }
}