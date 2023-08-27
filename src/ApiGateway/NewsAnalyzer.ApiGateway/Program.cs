using ApiGateway.Extensions;
using Grpc.Net.Client;
using static NewsAnalyzer.Application.NewsService.ApplicationNews;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddAuthenticationServices(builder.Configuration);
builder.Services.AddUserDbContext(builder.Configuration);
builder.Services.AddScoped(sp => new ApplicationNewsClient(GrpcChannel.ForAddress("https://localhost:54120")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
