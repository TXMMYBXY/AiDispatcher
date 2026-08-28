using AiDispatcher.Domain.Entities;
using AiDispatcher.Domain.Enums;
using AiDispatcher.Infrastructure.Configuration;
using Google.GenAI;
using Google.GenAI.Types;
using Microsoft.Extensions.Options;

namespace AiDispatcher.Infrastructure.Senders;

public class GoogleSender : IRequestSender
{
    private readonly GoogleSettings _settings;
    
    public GoogleSender(IOptions<GoogleSettings> settings)
    {
        _settings = settings.Value;
    }
    
    public AgentType AgentType => AgentType.Gemini;
    
    public async Task<AiResponse> SendRequestAsync(AiRequest request, CancellationToken ct = default)
    {
        var client = new Client(apiKey: _settings.ApiKey);

        GenerateContentResponse response = null;
        
        try
        {
            response = await client.Models.GenerateContentAsync(
                model: "gemini-3.6-flash",
                contents: request.Content,
                cancellationToken: ct);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        return new AiResponse(requestId: request.Id, content: response.Text);
    }
}