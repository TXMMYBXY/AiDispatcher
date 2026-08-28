using AiDispatcher.Domain.Enums;

namespace AiDispatcher.Shared.Models;

public class GoogleModel
{
    public Dictionary<AgentType, string> models = new()
    {
        { AgentType.Gemini, "gemini-2.0-flash" },
    };
}