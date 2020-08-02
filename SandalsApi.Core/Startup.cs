using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SandalsApi.Core.Other;
using SandalsApi.Core.Utilities;

namespace SandalsApi.Core
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
            services.AddCors(options =>
            {
                options.AddPolicy(name: "origins",
                              builder =>
                              {
                                  builder.WithOrigins("http://localhost:8817").AllowAnyHeader().AllowCredentials().AllowAnyMethod();
                              });
            });

            services.AddSignalR();
            services.AddControllers();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme =  "BasicAuthenticationScheme";
            })
            .AddScheme<ValidateBase64CredentialsSchemeOptions, BasicAuthenticationHandler>
                    ("BasicAuthenticationScheme", op => { });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else 
            {
                app.UseHsts();
            }

            app.UseRouting();
            app.UseCors("origins");
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "wwwroot")),
                RequestPath = "/public"
            });

            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseHttpsRedirection();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<NotificationHub>("/hub");
            });

        }
    }
}
