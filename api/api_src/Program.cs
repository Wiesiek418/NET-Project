using Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddMongoInfrastructure();
builder.AddMqttInfrastructure();

builder.Services.AddControllers()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddControllers(options =>
{
    options.OutputFormatters.Add(new CsvHelperOutputFormatter());
});

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

// Build app

var app = builder.Build();

app.UseCors("VueApp");

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.Run();