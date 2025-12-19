using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PartnerManager.Infrastructure.Config;
using PartnerManager.Infrastructure.Data;
using PartnerManager.Infrastructure.Repositories;
using PartnerManager.Service.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Ensure legacy timestamp behavior to avoid UTC cast issues (optional, can be removed once all DateTimes are UTC)
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

// Explicitly load base and environment-specific settings (appsettings.{Environment}.json)
builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Bind configuration/environment into a strongly-typed config object
var cconfig = new CConfig
{
    DefaultConnection = builder.Configuration.GetConnectionString("DefaultConnection"),
    JwtKey = builder.Configuration["Jwt:Key"],
    JwtIssuer = builder.Configuration["Jwt:Issuer"],
    JwtAudience = builder.Configuration["Jwt:Audience"],
    JwtExpireMinutes = int.TryParse(builder.Configuration["Jwt:ExpireMinutes"], out var expMin) ? expMin : 60,
    Environment = builder.Environment.EnvironmentName
};
builder.Services.AddSingleton(cconfig);

// MVC/JSON
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});
builder.Services.AddEndpointsApiExplorer();

// Swagger + JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PartnerManager API", Version = "v1" });
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] then your token."
    };
    c.AddSecurityDefinition("Bearer", securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    });
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(cconfig.DefaultConnection ?? builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories & Services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPartnerRepository, PartnerRepository>();
builder.Services.AddScoped<IActivityRepository, ActivityRepository>();

builder.Services.AddScoped<IAuthProvider, LocalAuthProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Authentication - JWT
var jwtKey = cconfig.JwtKey ?? builder.Configuration["Jwt:Key"] ?? string.Empty;
var keyBytes = Encoding.UTF8.GetBytes(jwtKey);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = cconfig.JwtIssuer ?? builder.Configuration["Jwt:Issuer"],
        ValidAudience = cconfig.JwtAudience ?? builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        RoleClaimType = System.Security.Claims.ClaimTypes.Role,
        NameClaimType = "username"
    };
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("JwtBearer");
            logger.LogError(context.Exception, "JWT Authentication failed");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("JwtBearer");
            logger.LogInformation("JWT Token validated successfully");
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Global exception handler
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        var logger = context.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("GlobalException");
        logger.LogError(ex, "Unhandled exception: {Message}\n{StackTrace}", ex.Message, ex.StackTrace);
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new { error = ex.Message, type = ex.GetType().Name });
    }
});

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Initialize DB (apply migrations and seed)
try
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("Startup");
    logger.LogInformation("Starting database initialization...");
    await DbInitializer.InitializeAsync(services);
    logger.LogInformation("Database initialization completed");
}
catch (Exception ex)
{
    var logger = app.Services.CreateScope().ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Startup");
    logger.LogError(ex, "Error during database initialization");
    throw;
}

await app.RunAsync();