using AiDispatcher.Infrastructure.AiDispatcher;
using AiDispatcher.Shared.Contracts.Events;
using Google.Apis.Util;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace AiDispatcher.Infrastructure.Consumers;

public class RequestConsumer : IConsumer<AiRoutingRequest>
{
    private readonly ILogger<RequestConsumer> _logger;
    private readonly IProviderDispatcher _providerDispatcher;

    public RequestConsumer(ILogger<RequestConsumer> logger, IProviderDispatcher providerDispatcher)
    {
        _logger = logger;
        _providerDispatcher = providerDispatcher;
    }
    
    public async Task Consume(ConsumeContext<AiRoutingRequest> context)
    {
        var request = context.Message;
        
        request.ThrowIfNull(nameof(request));
        
        _logger.LogInformation("Received request with id: {Request}", request.Request.Id);
        
        await _providerDispatcher.DispatchAsync(request.Request);
    }
}