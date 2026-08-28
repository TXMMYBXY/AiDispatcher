using AiDispatcher.Domain.Entities;
using AiDispatcher.Domain.Enums;

namespace AiDispatcher.Infrastructure.Senders;

public interface IRequestSender
{
    AgentType AgentType { get; }
    
    Task<AiResponse> SendRequestAsync(AiRequest request, CancellationToken ct = default);
}