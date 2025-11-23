using CVRecognizingService.API.Midleware.Exceptions;
using CVRecognizingService.API.Policies;
using CVRecognizingService.Application.Mappings;
using CVRecognizingService.Application.ServiceExctensions;
using CVRecognizingService.Application.UseCases.Commands.Documents;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

// Configure CORS policy
var allowedOrigins = builder.Configuration["Cors:AllowedOrigins"]?.Split(',', StringSplitOptions.RemoveEmptyEntries) ?? new[] { "http://localhost:5173" };
builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultCorsPolicy", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

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
            if (c.Request.Cookies.TryGetValue("key", out var cookieToken) && !string.IsNullOrWhiteSpace(cookieToken))
            {
                c.Token = cookieToken;
            }
            else if (c.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                var header = authHeader.ToString();
                if (header.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    c.Token = header.Substring("Bearer ".Length).Trim();
                }
            }
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

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        c.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_0;
    });
    app.UseSwaggerUI();
}

app.UseRouting();

// Apply named CORS policy
app.UseCors("DefaultCorsPolicy");

app.UseExceptionHandlerMiddleware();

app.UseHttpsRedirection();

// Ensure authentication is enabled
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
