using AiDispatcher.Domain.Entities;
using AiDispatcher.Domain.Enums;
using AiDispatcher.Infrastructure.Configuration;
using Google.GenAI;
using Google.GenAI.Types;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AiDispatcher.Infrastructure.Senders;

public class GoogleSender : IRequestSender
{
    private readonly ILogger<GoogleSender> _logger;
    private readonly GoogleSettings _settings;

    private static readonly Dictionary<IntentType, string[]> AvailableModels = new()
    {
        { IntentType.Reference,
            ["gemini-3.1-flash-lite", "gemini-3.5-flash-lite", "gemini-3.5-flash", "gemini-3.6-flash",] },
        { IntentType.Image, 
            ["gemini-2.5-flash-image", "gemini-3-pro-image", "gemini-3.1-flash-lite-image", "gemini-3.1-flash-image"] },
        { IntentType.Audio, 
            ["gemini-2.5-flash-preview-tts", "gemini-3.1-flash-tts-preview", "gemini-3.1-flash-live-preview"] }
    };

    
    public GoogleSender(ILogger<GoogleSender> logger, IOptions<GoogleSettings> settings)
    {
        _logger = logger;
        _settings = settings.Value;
    }
    
    public ModelProvider ModelProvider => ModelProvider.Google;
    
    public async Task<AiResponse> SendRequestAsync(AiRequest request, CancellationToken ct = default)
    {
        var client = new Client(apiKey: _settings.ApiKey);
        GenerateContentResponse response;
        
        try
        {
            response = await client.Models.GenerateContentAsync(
                model: "gemini-3.5-flash-lite",
                contents: request.Content,
                cancellationToken: ct);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Error}", e.Message);
            
            request.MarkAsFailed();
            
            throw;
        }
        
        return new AiResponse(requestId: request.Id, chatId: request.UserId, content: response.Text);
    }
}