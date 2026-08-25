using AiDispatcher.Domain.Entities;
using AiDispatcher.Domain.Enums;
using AiDispatcher.Infrastructure.Clients;
using AiDispatcher.Infrastructure.Senders;

namespace AiDispatcher.Infrastructure.AiDispatcher;

public class AgentDispatcher : IAgentDispatcher
{
    private readonly IReadOnlyDictionary<AgentType, IRequestSender> _requestSenders;
    private readonly IWebhookClient _webhookClient;
    
    public AgentDispatcher(IEnumerable<IRequestSender> senders, IWebhookClient webhookClient)
    {
        _requestSenders = senders.ToDictionary(x => x.AgentType);
        _webhookClient = webhookClient;
    }

    public async Task DispatchAsync(AiRequest request)
    {
        var response = await _requestSenders[request.AgentId].SendRequestAsync(request);
        
        await _webhookClient.SendResponseAsync(request.WebhookUrl, response);
    }
}