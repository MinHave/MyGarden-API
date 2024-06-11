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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

// Configure Identity
builder.Services.AddIdentity<ApiUser, IdentityRole>()
    .AddEntityFrameworkStores<ApiDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IFtpService, FtpService>();

// Register HttpClient
builder.Services.AddHttpClient();

// Register JwtFactory
builder.Services.AddScoped<IJwtFactory, JwtFactory>();

// Register IHttpContextAccessor
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Register FtpOptions configuration
builder.Services.Configure<FtpOptions>(builder.Configuration.GetSection("FtpOptions"));

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
var jwtAppSettingOptions = builder.Configuration.GetSection(nameof(JwtIssuerOptions));
builder.Services.Configure<JwtIssuerOptions>(options =>
{
    options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
    options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
    options.SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtAppSettingOptions["SecretKey"])), SecurityAlgorithms.HmacSha256);
});

var app = builder.Build();

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
