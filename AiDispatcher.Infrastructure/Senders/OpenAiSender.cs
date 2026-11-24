using AiDispatcher.Domain.Entities;
using AiDispatcher.Domain.Enums;
using AiDispatcher.Infrastructure.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenAI.Responses;
#pragma warning disable OPENAI001

namespace AiDispatcher.Infrastructure.Senders;

public class OpenAiSender : IProviderSender
{
    private readonly ILogger<OpenAiSender> _logger;
    private readonly OpenAiConfiguration _configuration;
    private readonly ResponsesClient _client;

    private static readonly Dictionary<IntentType, string[]> AvailableModels = new()
    {
        { IntentType.Reference, [""] },
        { IntentType.Image, [""] },
        { IntentType.Audio, [""] }
    };

    public OpenAiSender(ILogger<OpenAiSender> logger, IOptions<OpenAiConfiguration> configuration)
    {
        _logger = logger;
        _configuration = configuration.Value;
        _client = new ResponsesClient(apiKey: _configuration.ApiKey);
    }
    
    public ModelProvider ModelProvider => ModelProvider.OpenAi;
    
    public async Task<AiResponse> SendRequestAsync(AiRequest request, CancellationToken ct = default)
    {
        ResponseResult response;
        
        try
        {
            response = await _client.CreateResponseAsync(
                model: "gpt-5.2",
                userInputText: request.Content,
                cancellationToken: ct);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Error}", e.Message);
            
            request.MarkAsFailed();
            
            throw;
        }

        return new AiResponse(requestId: request.Id, chatId: request.UserId, content: response.GetOutputText());
    }
}