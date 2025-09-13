using CVRecognizingService.Application.ServiceExctensions;
using CVRecognizingService.API.Midleware.Exceptions;
using CVRecognizingService.Application.Mappings;
using CVRecognizingService.Application.UseCases.Commands.Documents;
using Microsoft.AspNetCore.Authentication.BearerToken;
using CVRecognizingService.API.Policies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

//Config database service with Repositories
builder.Services.AddRepositories();

var mongoConnectionString = builder.Configuration.GetConnectionString("MongoDb");

builder.Services.AddDbConnectionSettings(builder.Configuration);
builder.Services.AddDbContext();

//Config AI service
builder.Services.AddGeminiAI(builder.Configuration.GetSection("API_KEY").Value);

//Confin validation services
builder.Services.AddValidation();


//Config Controllers Services
builder.Services.AddServices();

//Config AutoMapper
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<MappingProfile>();
});

//Config CQRS
builder.Services.AddMediatR(config => 
    config.RegisterServicesFromAssembly(typeof(CreateDocumentCommandHandler).Assembly));
builder.Services.AddControllers();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:AccessTokenSecret"]!)),
        ValidateActor = true,
        ValidateIssuer = true,
        ValidateAudience = false,
        RequireExpirationTime = true,
        ValidateIssuerSigningKey = true
    };
    options.Events = new JwtBearerEvents()
    {
        OnMessageReceived = c =>
        {
            c.Token = c.Request.Cookies["key"];
            return Task.CompletedTask;
        }
    };
});
builder.Services.AddAuthorization(option =>
{
    option.AddPolicy(Policies.RequireStaff,
        policy => policy.Requirements.Add(new RolesRequirement([Roles.Admin, Roles.User])));
});

builder.Services.AddSwaggerWithAuth();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        c.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_0;
    });
    app.UseSwaggerUI();
}

app.UseExceptionHandlerMiddleware();

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
