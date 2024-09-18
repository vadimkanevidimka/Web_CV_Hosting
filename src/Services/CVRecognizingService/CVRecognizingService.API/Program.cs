using CVRecognizingService.Application.ServiceExctensions;
using CVRecognizingService.API.Midleware.Exceptions;
using CVRecognizingService.Application.Mappings;
using System;
using CVRecognizingService.Application.UseCases.Commands.Document;

var builder = WebApplication.CreateBuilder(args);

///Config database service with Repositories
builder.Services.AddRepositories();

var mongoConnectionString = builder.Configuration.GetConnectionString("MongoDb");

if(!string.IsNullOrEmpty(mongoConnectionString))
    builder.Services.AddMongoDB(mongoConnectionString);



///Config AI service
///
builder.Services.AddGeminiAI(builder.Configuration.GetSection("API_KEY").Value);


///Confin validation services
///
builder.Services.AddValidation();


///Config Controllers Services
///
builder.Services.AddServices();

///Config AutoMapper
///
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<MappingProfile>();
});



///Config CQRS
///
builder.Services.AddMediatR(config => 
    config.RegisterServicesFromAssembly(typeof(CreateDocumentCommandHandler).Assembly));
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

app.UseExceptionHandlerMiddleware();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
