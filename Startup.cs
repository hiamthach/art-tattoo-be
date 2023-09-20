using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using art_tattoo_be.Domain.RoleBase;


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
      services.AddAutoMapper(typeof(RoleProfile));
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
    }
  }
}
