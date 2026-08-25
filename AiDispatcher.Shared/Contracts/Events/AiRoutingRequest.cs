using AiDispatcher.Domain.Entities;

namespace AiDispatcher.Shared.Contracts.Events;

public record AiRoutingRequest(Guid RequestId, AiRequest Request);