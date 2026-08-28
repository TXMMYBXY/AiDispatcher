using AiDispatcher.Infrastructure.AiDispatcher;
using AiDispatcher.Shared.Contracts.Events;
using Google.Apis.Util;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace AiDispatcher.Infrastructure.Consumers;

public class RequestConsumer : IConsumer<AiRoutingRequest>
{
    private readonly ILogger<RequestConsumer> _logger;
    private readonly IAgentDispatcher _agentDispatcher;

    public RequestConsumer(ILogger<RequestConsumer> logger, IAgentDispatcher agentDispatcher)
    {
        _logger = logger;
        _agentDispatcher = agentDispatcher;
    }
    
    public async Task Consume(ConsumeContext<AiRoutingRequest> context)
    {
        var request = context.Message;
        
        request.ThrowIfNull(nameof(request));
        
        _logger.LogInformation("Received request with id: {Request}", request.RequestId);
        
        await _agentDispatcher.DispatchAsync(request.Request);
    }
}