using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using SendGrid;
using System.Data;
using System.Text;
using NETCleanArchApi.Middleware;
using NETCleanArch.Application.Interfaces.IRepositories;
using NETCleanArch.Application.Interfaces.ISecurity;
using NETCleanArch.Application.Interfaces.IServices;
using NETCleanArch.Application.Services;
using NETCleanArchInfrastructure.Data; // Add this using
using NETCleanArchInfrastructure.Notifications.ExternalService;
using NETCleanArchInfrastructure.Notifications.IExternalService;
using NETCleanArchInfrastructure.Repositories;
using NETCleanArchInfrastructure.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
// Option 1: Register DapperDbContext (Recommended)
builder.Services.AddScoped<DapperDbContext>();

// Option 2: Or keep IDbConnection registration if you prefer
// builder.Services.AddScoped<IDbConnection>(_ =>
//     new NpgsqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IEmailService, SendGridEmailService>();

builder.Services.AddScoped<ISendGridClient>(_ =>
    new SendGridClient(builder.Configuration["SendGrid:ApiKey"]));

// JWT Authentication
var jwtSecret = builder.Configuration["Jwt:Secret"];
if (string.IsNullOrEmpty(jwtSecret))
{
    throw new InvalidOperationException("JWT Secret is not configured.");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSecret))
        };
        options.SaveToken = true;
    });

builder.Services.AddAuthorization();

// Add CORS if needed
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Add logging
builder.Services.AddLogging();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseAuthentication();
app.UseMiddleware<JwtMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();