using NlpService.Core.Abstractions;
using NlpService.Data;
using NlpService.Data.Abstractions;
using NlpService.NerService.Services;
using NlpService.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddSingleton<INerService, CatalystNerService>();
builder.Services.AddScoped<INlpUnitOfWork, NlpUnitOfWork>();
builder.Services.AddServicesConfiguration(builder.Configuration);

var app = builder.Build();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
