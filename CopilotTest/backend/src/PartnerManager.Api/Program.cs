using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PartnerManager.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger with JWT support
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
        Description = "Enter 'Bearer' [space] and then your valid token in the text input below."
    };
    c.AddSecurityDefinition("Bearer", securityScheme);
    var securityReq = new OpenApiSecurityRequirement
    {
        { securityScheme, new string[] { } }
    };
    c.AddSecurityRequirement(securityReq);
});

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories & Services
builder.Services.AddScoped<PartnerManager.Infrastructure.Repositories.IUserRepository, PartnerManager.Infrastructure.Repositories.UserRepository>();
builder.Services.AddScoped<PartnerManager.Service.Services.IAuthService, PartnerManager.Service.Services.AuthService>();
builder.Services.AddScoped<PartnerManager.Infrastructure.Repositories.IPartnerRepository, PartnerManager.Infrastructure.Repositories.PartnerRepository>();
builder.Services.AddScoped<PartnerManager.Infrastructure.Repositories.IActivityRepository, PartnerManager.Infrastructure.Repositories.ActivityRepository>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

// Authentication - JWT
var jwtKey = builder.Configuration["Jwt:Key"];
var keyBytes = System.Text.Encoding.UTF8.GetBytes(jwtKey ?? string.Empty);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(keyBytes),
        RoleClaimType = System.Security.Claims.ClaimTypes.Role,
        NameClaimType = "username"
    };
});

// Authorization
builder.Services.AddAuthorization();

var app = builder.Build();

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
        logger.LogError(ex, "Unhandled exception");
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new { error = "An unexpected error occurred." });
    }
});

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Initialize DB (apply migrations and seed)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await PartnerManager.Infrastructure.Data.DbInitializer.InitializeAsync(services);
}

app.Run();
