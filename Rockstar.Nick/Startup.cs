using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Rockstar.Nick.Context;

namespace Rockstar.Nick
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMemoryCache();
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Nick API", Version = "v1" });
        c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
        c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
        {
          Name = "Authorization",
          Type = SecuritySchemeType.Http,
          Scheme = "basic",
          In = ParameterLocation.Header,
          Description = "Basic Authorization header using the Bearer scheme."
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "basic"
                                }
                            },
                            new string[] {}
                    }
                });
      });
      services.AddAuthentication("BasicAuthentication").AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

      services.AddControllers();

      byte[] data = Convert.FromBase64String(Configuration.GetConnectionString("MusicConnectString"));
      string decodedString = Encoding.UTF8.GetString(data);
      services.AddDbContext<MusicContext>(options => options.UseSqlServer(decodedString));
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        //app.UseSwagger("swagger/index/swaggers.sjon", ;
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
          c.SwaggerEndpoint("./v1/swagger.json", "Nick API");

          c.DisplayRequestDuration();
        });

        //app.UseSwaggerUI(options => options.SwaggerEndpoint("./v1/swagger.json", "Nick API"));
        //app.UseSwaggerUI(options => options.DisplayRequestDuration());

        app.UseDeveloperExceptionPage();
      }

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });


    }
  }
}
