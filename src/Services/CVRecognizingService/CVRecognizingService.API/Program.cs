using CVRecognizingService.Infrastructure.DataAccess;
using DotnetGeminiSDK;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


var mongoConnectionString = builder.Configuration.GetConnectionString("MongoDb")!;

builder.Services.AddDbContext<CVRecDBContext>(x => x
    .EnableSensitiveDataLogging()
    .UseMongoDB(mongoConnectionString, "Documents")
);


builder.Services.AddGeminiClient(config =>
{
    config.ApiKey = builder.Configuration.GetSection("API_KEY").Value;
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();






var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
