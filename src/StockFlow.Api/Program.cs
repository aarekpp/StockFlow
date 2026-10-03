using StockFlow.Api.Middleware;
using StockFlow.Application;
using StockFlow.Infrastructure;
using StockFlow.Infrastructure.Persistence;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("MiniErpDb") ?? throw new InvalidOperationException("Connection string 'MiniErpDb' is not configured.");

builder.Services.AddInfrastructure(connectionString);
builder.Services.AddApplication();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services
    .AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var apiXml = Path.Combine(AppContext.BaseDirectory, $"{typeof(Program).Assembly.GetName().Name}.xml");
    if (File.Exists(apiXml))
    {
        options.IncludeXmlComments(apiXml);
    }

    var applicationXml = Path.Combine(AppContext.BaseDirectory, "MiniErp.Application.xml");
    if (File.Exists(applicationXml))
    {
        options.IncludeXmlComments(applicationXml);
    }
});

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    await StockFlowDbInitializer.SeedAsync(app.Services);
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
