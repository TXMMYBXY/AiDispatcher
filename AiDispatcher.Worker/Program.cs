using AiDispatcher.Infrastructure;
using AiDispatcher.Infrastructure.Configuration;
using AiDispatcher.Worker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<GoogleConfiguration>(
    builder.Configuration.GetSection(nameof(GoogleConfiguration)));
builder.Services.Configure<OpenAiConfiguration>(
    builder.Configuration.GetSection(nameof(OpenAiConfiguration)));

var googleApiKey = Environment.GetEnvironmentVariable("GOOGLE_APIKEY");
var openAiApiKey = Environment.GetEnvironmentVariable("OPENAI_APIKEY");
if (!string.IsNullOrWhiteSpace(googleApiKey))
{
    builder.Configuration[$"GoogleConfiguration:{nameof(BaseConfiguration.ApiKey)}"] = googleApiKey;
}
if (!string.IsNullOrWhiteSpace(openAiApiKey))
{
    builder.Configuration[$"OpenAiConfiguration:{nameof(BaseConfiguration.ApiKey)}"] = openAiApiKey;
}

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();