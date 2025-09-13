using AuthService.Presentation.Exstensions;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDataAccessLayer(builder.Configuration);

builder.Services
    .AddBusinessLogicLayer(builder.Configuration);

builder.Services
    .AddPresentationLayer(builder.Configuration);

var app = builder.Build();

app.StartApplication();
