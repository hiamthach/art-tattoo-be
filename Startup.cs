using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using art_tattoo_be.Application.Middlewares;
using art_tattoo_be.Domain.RoleBase;
using art_tattoo_be.Infrastructure.Repository;

namespace MyApp
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      // Configure services (e.g., database, authentication, etc.)
      services.AddAutoMapper(typeof(Startup));
      services.AddScoped<IRoleBaseRepository, RoleBaseRepository>();
      services.AddCors(options =>
      {
        options.AddPolicy("AllowAllOrigins",
            builder =>
            {
              builder.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
      });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        // Configure error handling for production
      }

      // Configure middleware (e.g., routing, authentication, etc.)
      app.UseMiddleware<ModelStateValidationMiddleware>();

      app.UseRouting();
      // ...

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers(); // Map your controllers
      });
    }
  }
}
