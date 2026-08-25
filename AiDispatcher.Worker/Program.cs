using AiDispatcher.Infrastructure;
using AiDispatcher.Infrastructure.Configuration;
using AiDispatcher.Worker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<GoogleSettings>(builder.Configuration.GetSection("GoogleSettings"));

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();