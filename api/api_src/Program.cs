using System.Text.Json;

using Infrastructure.SignalR;
using Infrastructure.Data.MongoDB;
using Infrastructure.Mqtt;
using Infrastructure.Formatters.Csv;
using Domains.Blockchain.Infrastructure;
using Domains.Sensors.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddMongoInfrastructure();
builder.AddMqttInfrastructure();
builder.AddBlockchainInfrastructure();
builder.AddSensorsInfrastructure();

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);

builder.Services.AddControllers(options => { options.OutputFormatters.Add(new CsvHelperOutputFormatter()); });

builder.Services.AddCors(options =>
{
    options.AddPolicy("VueApp", policy =>
    {
        policy.WithOrigins("http://localhost:8080", "https://localhost:8080")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddOpenApi();
builder.Services.AddSignalR()
    .AddJsonProtocol(options => { options.PayloadSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; });

// Build app

var app = builder.Build();

app.UseCors("VueApp");

app.MapControllers();
app.MapHub<NotificationHub>("/api/notifications");
app.MapHealthChecks("/healthy");

if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.Run();