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
        
        services.Configure<GoogleSettings>(configuration.GetSection(nameof(GoogleSettings)));
        
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
        services.AddScoped<IRequestSender, GoogleSender>();

        services.AddHttpClient<IWebhookClient, WebhookClient>();

        services.AddScoped<IReadOnlyDictionary<ModelProvider, IRequestSender>>(sp =>
            new Dictionary<ModelProvider, IRequestSender>()
            {
                { ModelProvider.Google, sp.GetRequiredService<GoogleSender>()}
            });
        
        return services;
    }
}