using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProfileService.Application.Extensions;
using ProfileService.Application.MappingProfiles;
using ProfileService.Application.UseCases.CommandHandlers;
using ProfileService.Application.Validators;
using ProfileService.Infastructure.DbAccess;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddMediatR(config =>
    config.RegisterServicesFromAssembly(typeof(CreateApplicantProfileCommandHandler).Assembly));

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
});

builder.Services.AddSwaggerWithAuth();

// Регистрация валидаторов из сборки
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddDbContext<ProfileDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddRepositories();

builder.Services.AddCors();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AllowNullCollections = true;
},
typeof(ApplicantProfieMapProfile));

var app = builder.Build();

// Apply migrations automatically
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var dbContext = scope.ServiceProvider.GetRequiredService<ProfileDbContext>();
    
    try
    {
        logger.LogInformation("Starting database migration...");
        
        // Check if ApplicantProfiles table exists
        var canConnect = await dbContext.Database.CanConnectAsync();
        logger.LogInformation($"Can connect to database: {canConnect}");
        
        if (canConnect)
        {
            // Check if the main table exists
            var tableExists = false;
            try
            {
                tableExists = await dbContext.ApplicantProfiles.AnyAsync();
            }
            catch
            {
                // Table doesn't exist, this is expected
                tableExists = false;
            }
            
            if (!tableExists)
            {
                logger.LogInformation("ApplicantProfiles table does not exist. Creating database schema...");
                
                // Delete migration history table if it exists
                await dbContext.Database.ExecuteSqlRawAsync(
                    "DROP TABLE IF EXISTS \"__EFMigrationsHistory\"");
                
                // Create all tables
                await dbContext.Database.EnsureCreatedAsync();
                logger.LogInformation("Database schema created successfully.");
            }
            else
            {
                logger.LogInformation("Database tables already exist.");
            }
        }
        
        logger.LogInformation("Database migration completed successfully.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while migrating the database.");
        throw;
    }
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

if (app.Environment.IsDevelopment())
{
    if (app.Environment.IsDevelopment())
    {
        app.UseHttpsRedirection();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Profile Service API v1");
            c.RoutePrefix = "swagger";
        });
    }
}

app.UseCors(cfg =>
{
    cfg.AllowAnyOrigin();
    cfg.AllowAnyHeader();
    cfg.AllowAnyMethod();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();