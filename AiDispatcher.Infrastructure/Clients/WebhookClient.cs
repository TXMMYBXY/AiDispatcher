using System.Text;
using System.Text.Json;
using AiDispatcher.Domain.Entities;
using AiDispatcher.Shared.Contracts.Events;
using Microsoft.Extensions.Logging;

namespace AiDispatcher.Infrastructure.Clients;

public class WebhookClient : IWebhookClient
{
    private readonly ILogger<WebhookClient> _logger;
    private readonly HttpClient _httpClient;

    public WebhookClient(ILogger<WebhookClient> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }
    
    public async Task SendResponseAsync(string uri, AiResponse response)
    {
        _logger.LogInformation($"Sending response to {uri}");
        _logger.LogInformation(response.Content);
        
        var requestJson = JsonSerializer.Serialize(response);
        using var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");
        using var result = await _httpClient.PostAsync(uri, requestContent);

        if (!result.IsSuccessStatusCode)
        {
            var errorContent = await result.Content.ReadAsStringAsync();
            var errorResponse = new ErrorResponse();
            try
            {
                errorResponse = JsonSerializer.Deserialize<ErrorResponse>(errorContent);
            }
            catch (JsonException ex)
            {
                throw new HttpRequestException(
                    $"Request failed with status {result.StatusCode}, exception: {ex.Message}",
                    null,
                    result.StatusCode
                );
            }
            
            throw new HttpRequestException(
                $"Request failed with status {result.StatusCode}, message: {errorResponse.Message}",
                null,
                result.StatusCode
            );
        }
    }

    public async Task SendResultAsync(string uri, AiGenerationResult result)
    {
        var requestJson = JsonSerializer.Serialize(result);
        using var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");
        using var response = await _httpClient.PostAsync(uri, requestContent);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException(
                $"Webhook request failed with status {response.StatusCode}: {errorContent}",
                null,
                response.StatusCode);
        }
    }
}