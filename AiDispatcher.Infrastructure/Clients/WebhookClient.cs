using System.Text;
using System.Text.Json;
using AiDispatcher.Domain.Entities;

namespace AiDispatcher.Infrastructure.Clients;

public class WebhookClient : IWebhookClient
{
    private readonly HttpClient _httpClient;

    public WebhookClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task SendResponseAsync(string uri, AiResponse response)
    {
        var requestJson = JsonSerializer.Serialize(response.Content);
        var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");
        var result = await _httpClient.PostAsync(uri, requestContent);

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
                    $"Request failed with status {result.StatusCode}",
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
}