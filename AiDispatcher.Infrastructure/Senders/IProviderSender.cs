using AiDispatcher.Domain.Entities;
using AiDispatcher.Domain.Enums;

namespace AiDispatcher.Infrastructure.Senders;

public interface IProviderSender
{
    ModelProvider ModelProvider { get; }
    
    Task<AiResponse> SendRequestAsync(AiRequest request, CancellationToken ct = default);
}