using AiDispatcher.Domain.Enums;
using AiDispatcher.Infrastructure.AiDispatcher;
using AiDispatcher.Infrastructure.Configuration;
using AiDispatcher.Infrastructure.Consumers;
using AiDispatcher.Infrastructure.Senders;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AiDispatcher.Infrastructure.Clients;

namespace AiDispatcher.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";
        var rabbitUser = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "guest";
        var rabbitPass = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? "guest";
        
        services.Configure<GoogleConfiguration>(configuration.GetSection(nameof(GoogleConfiguration)));
        services.Configure<OpenAiConfiguration>(configuration.GetSection(nameof(OpenAiConfiguration)));
        
        services.AddMassTransit(options =>
        {
            options.AddConsumer<RequestConsumer>();
            options.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(rabbitHost, "/", host =>
                {
                    host.Username(rabbitUser);
                    host.Password(rabbitPass);
                });
        
                cfg.ConfigureEndpoints(ctx);
            });
        });
        
        services.AddScoped<IProviderDispatcher, ProviderDispatcher>();
        
        services.AddScoped<GoogleSender>();
        services.AddScoped<OpenAiSender>();
        services.AddScoped<IProviderSender, GoogleSender>();
        services.AddScoped<IProviderSender, OpenAiSender>();

        services.AddHttpClient<IWebhookClient, WebhookClient>();

        services.AddScoped<IReadOnlyDictionary<ModelProvider, IProviderSender>>(sp =>
            new Dictionary<ModelProvider, IProviderSender>()
            {
                { ModelProvider.Google, sp.GetRequiredService<GoogleSender>()},
                { ModelProvider.OpenAi, sp.GetRequiredService<OpenAiSender>()},
            });
        
        return services;
    }
}