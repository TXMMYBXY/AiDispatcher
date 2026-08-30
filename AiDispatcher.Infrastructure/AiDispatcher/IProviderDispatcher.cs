using AiDispatcher.Domain.Entities;

namespace AiDispatcher.Infrastructure.AiDispatcher;

public interface IProviderDispatcher
{
    Task DispatchAsync(AiRequest request);
}