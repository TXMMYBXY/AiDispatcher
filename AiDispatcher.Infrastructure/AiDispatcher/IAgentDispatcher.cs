using AiDispatcher.Domain.Entities;

namespace AiDispatcher.Infrastructure.AiDispatcher;

public interface IAgentDispatcher
{
    Task DispatchAsync(AiRequest request);
}