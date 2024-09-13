using DotnetGeminiSDK;
using CVRecognizingService.Domain.Entities;
using CVRecognizingService.Domain.Abstracts.Repo;
using CVRecognizingService.Infrastructure.DataAccess.Repositories;
using CVRecognizingService.Application.ServiceExctensions;

var builder = WebApplication.CreateBuilder(args);

///Config database service with Repositories

var mongoConnectionString = builder.Configuration.GetConnectionString("MongoDb");
builder.Services.AddTransient<IRepository<BaseDocument>, DocumentRepository>();

if(!string.IsNullOrEmpty(mongoConnectionString))
    builder.Services.AddMongoDB(mongoConnectionString);



///Config AI service
///
builder.Services.AddGeminiClient(config =>
{
    config.ApiKey = builder.Configuration.GetSection("API_KEY").Value;
});


///Confin validation services
///
builder.Services.AddValidation();


///Config Controllers Services
///
builder.Services.AddServices();

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
