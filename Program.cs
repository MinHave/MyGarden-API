using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MyGarden_API.Data;
using MyGarden_API.Services;
using MyGarden_API.Models.Entities;
using MyGarden_API.API.Auth;
using MyGarden_API.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MyGarden_API.API.Models;
using MyGarden_API.Services.Interfaces;
using MyGarden_API.Repositories;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MyGarden_API.ViewModels.Mappings;
using MyGarden_API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// Configure Identity
builder.Services.AddIdentity<ApiUser, IdentityRole>()
    .AddEntityFrameworkStores<ApiDbContext>()
    .AddDefaultTokenProviders();

// Configure JWT Authentication
var jwtAppSettingOptions = builder.Configuration.GetSection(nameof(JwtIssuerOptions));
var secretKey = jwtAppSettingOptions["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey is not configured.");

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
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],
        ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey))
    };
});

builder.Services.AddScoped(typeof(IRepositoryDesignPattern<>), typeof(RepositoryDesignPattern<>));
builder.Services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IFtpService, FtpService>();
builder.Services.AddScoped<IPlantService, PlantService>();
builder.Services.AddScoped<IGardenService, GardenService>();
builder.Services.AddScoped<IGardenRepository, GardenRepository>();
builder.Services.AddScoped<IGardenPlantRepository, GardenPlantRepository>();

// Register HttpClient
builder.Services.AddHttpClient();

// Register JwtFactory
builder.Services.AddScoped<IJwtFactory, JwtFactory>();

// Register IHttpContextAccessor
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Register FtpOptions configuration
builder.Services.Configure<FtpOptions>(builder.Configuration.GetSection("FtpOptions"));

// Register AutoMapper
builder.Services.AddAutoMapper(typeof(EntityToViewModelMappingProfile));

// Add environment-specific services
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddScoped<IMailSenderService, DummyMailSenderService>();
}
else
{
    builder.Services.AddScoped<IMailSenderService, SimpleSmtpMailSenderService>();
}

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseSqlServer(connectionString));

// Configure JwtIssuerOptions
builder.Services.Configure<JwtIssuerOptions>(options =>
{
    options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
    options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
    options.SigningCredentials = new SigningCredentials(
        new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
        SecurityAlgorithms.HmacSha256
    );
});

var app = builder.Build();

// Build the service provider
var serviceProvider = builder.Services.BuildServiceProvider();

// Populate the accounts
DefaultData.PopulateAccounts(serviceProvider).Wait();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseCors(policy => policy
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
}
else
{
    app.UseCors(policy => policy
        .WithOrigins("https://tavsogmatias.com")
        .AllowAnyMethod()
        .AllowAnyHeader());
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
