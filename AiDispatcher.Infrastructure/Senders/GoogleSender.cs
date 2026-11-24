using AiDispatcher.Domain.Entities;
using AiDispatcher.Domain.Enums;
using AiDispatcher.Infrastructure.Configuration;
using Google.GenAI;
using Google.GenAI.Types;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AiDispatcher.Infrastructure.Senders;

public class GoogleSender : IProviderSender
{
    private readonly ILogger<GoogleSender> _logger;
    private readonly GoogleConfiguration _configuration;
    private readonly Client _client;

    private static readonly Dictionary<IntentType, string[]> AvailableModels = new()
    {
        { IntentType.Reference,
            ["gemini-3.1-flash-lite", "gemini-3.5-flash-lite", "gemini-3.5-flash", "gemini-3.6-flash",] },
        { IntentType.Image, 
            ["gemini-2.5-flash-image", "gemini-3-pro-image", "gemini-3.1-flash-lite-image", "gemini-3.1-flash-image"] },
        { IntentType.Audio, 
            ["gemini-2.5-flash-preview-tts", "gemini-3.1-flash-tts-preview", "gemini-3.1-flash-live-preview"] }
    };
    
    public GoogleSender(ILogger<GoogleSender> logger, IOptions<GoogleConfiguration> configuration)
    {
        _logger = logger;
        _configuration = configuration.Value;
        _client = new Client(apiKey: _configuration.ApiKey);
    }
    
    public ModelProvider ModelProvider => ModelProvider.Google;
    
    public async Task<AiResponse> SendRequestAsync(AiRequest request, CancellationToken ct = default)
    {
        GenerateContentResponse response;
        
        try
        {
            Content content;
            
            if (request.ImageData != null)
            {
                content = new Content
                {
                    Parts = new List<Part>
                    {
                        new() { Text = request.Content },
                        new()
                        {
                            InlineData = new Blob
                            {
                                MimeType = "image/jpeg",
                                Data = request.ImageData
                            }
                        }
                    }
                };
            }
            else
            {
                content = new Content()
                {
                    Parts = new()
                    {
                        new Part { Text = request.Content },
                    }
                };
            }
            
            response = await _client.Models.GenerateContentAsync(
                model: "gemini-3.5-flash",
                contents: content,
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