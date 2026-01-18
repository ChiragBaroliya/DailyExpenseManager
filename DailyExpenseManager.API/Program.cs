using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;
using AutoMapper;
using DailyExpenseManager.API.Mapping;
using DotNetEnv;
using DailyExpenseManager.Infrastructure.Mongo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using DailyExpenseManager.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Load .env file for secrets
Env.Load();

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Debug()
    .Enrich.FromLogContext()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();

// Add API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

// Register AutoMapper
builder.Services.AddAutoMapper(typeof(ApiMappingProfile));

// Register FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<DailyExpenseManager.Application.Validation.SampleCommandValidator>();

// Register MediatR (CQRS)
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DailyExpenseManager.Application.CQRS.ICommand<>).Assembly));

// Register MongoDB
builder.Services.AddMongoDb(builder.Configuration);

// JWT Authentication
var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY") ?? builder.Configuration["Jwt:Key"] ?? "YourSuperSecretKeyHere";
var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? builder.Configuration["Jwt:Issuer"] ?? "YourIssuer";
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// Register password hasher and JWT generator
builder.Services.AddScoped<Microsoft.AspNetCore.Identity.IPasswordHasher<DailyExpenseManager.Domain.Entities.User>, Microsoft.AspNetCore.Identity.PasswordHasher<DailyExpenseManager.Domain.Entities.User>>();
builder.Services.AddScoped<DailyExpenseManager.Application.Authentication.IJwtTokenGenerator, DailyExpenseManager.Application.Authentication.JwtTokenGenerator>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Use global exception handler
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
