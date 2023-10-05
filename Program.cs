using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using art_tattoo_be.Core.Jwt;
using art_tattoo_be.Infrastructure.Database;
using art_tattoo_be.Infrastructure.Cache;
using StackExchange.Redis;
using Microsoft.OpenApi.Models;
using art_tattoo_be.Core.Mail;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowAllOrigins",
    builder =>
    {
      builder.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
  options.AddDefaultPolicy(
    builder =>
    {
      builder.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddAuthorization();
builder.Services.AddAuthentication("Bearer").AddJwtBearer();

builder.Services.AddControllers().AddJsonOptions(options =>
{
  options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo { Title = "Art Tattoo Lover", Version = "v1" });

  // Add a bearer token to Swagger
  c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
  {
    Description = "JWT Authorization header using the Bearer scheme",
    Type = SecuritySchemeType.Http,
    Scheme = "bearer"
  });

  // Require the bearer token for all API operations
  c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
      {
          new OpenApiSecurityScheme
          {
              Reference = new OpenApiReference
              {
                  Type = ReferenceType.SecurityScheme,
                  Id = "Bearer"
              }
          },
          new string[] {}
      }
    });
});

builder.Services.AddScoped<ICacheService, CacheService>();

builder.Services.AddDbContext<ArtTattooDbContext>(options =>
{
  // log the connection string
  Console.WriteLine($"Connection string: {builder.Configuration.GetConnectionString("DatabaseConnection")}");
  options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection"));
});

Console.WriteLine($"Redis connection string: {builder.Configuration.GetConnectionString("RedisConnection")}");
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection")));

var smtpUsername = Environment.GetEnvironmentVariable("SMTP_EMAIL") ?? "default_username";
var smtpPassword = Environment.GetEnvironmentVariable("SMTP_PASSWORD") ?? "default_password";
builder.Services.AddSingleton<IMailService>(new MailService("smtp.gmail.com", 587, smtpUsername, smtpPassword));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IJwtService, JwtService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

DbInitializer.UseInitializeDatabase(app);

// allow all origins cors
app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
