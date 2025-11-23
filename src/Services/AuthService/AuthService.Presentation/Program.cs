using AuthService.Presentation.Exstensions;
using Microsoft.AspNetCore.HttpOverrides;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDataAccessLayer(builder.Configuration);

builder.Services
    .AddBusinessLogicLayer(builder.Configuration);

builder.Services
    .AddPresentationLayer(builder.Configuration);

var app = builder.Build();

var forwardOptions = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
};

// Разрешаем все прокси в docker-сетях
forwardOptions.KnownNetworks.Clear();
forwardOptions.KnownProxies.Clear();

app.UseForwardedHeaders(forwardOptions);
app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("ReqDbg");
    context.Request.EnableBuffering();
    using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
    var body = await reader.ReadToEndAsync();
    context.Request.Body.Position = 0;
    logger.LogInformation("Request Path: {Path}, Method: {Method}, Content {Content}", context.Request.Path, context.Request.Method, body);

    // Покажет какой endpoint (если маршрут нашёлся)
    var endpoint = context.GetEndpoint();
    logger.LogInformation("Endpoint: {Endpoint}", endpoint?.DisplayName ?? "NULL");

    await next();
});

app.StartApplication();
