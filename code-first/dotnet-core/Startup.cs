using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_core.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;
using Moon.AspNetCore.Authentication.Basic;
using System.Security.Claims;
using Swashbuckle.AspNetCore.Examples;

namespace dotnet_core
{
    public class Startup
    {
        private const string password = "test123";

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase());
            services.AddAuthorization();
            services.AddMvc();

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { 
                    Title = "Sample Movie DB",
                    Version = "0.0.1",
                    Description = "Sample movie DB to demonstrate swagger with code-first.",
                    License = new License { Name = "MIT License", Url = "https://example.com/license" }
                });

                // Set the comments path for the swagger json and ui.
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "dotnet-core.xml"); 
                c.IncludeXmlComments(xmlPath);

                // Enable Basic Authentication in Swagger
                c.AddSecurityDefinition("basic", new BasicAuthScheme { Type = "basic" });
                c.DocumentFilter<BasicAuthDocumentFilter>();

                c.DescribeAllEnumsAsStrings();
                c.DescribeStringEnumsInCamelCase();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            // WARNING! Never ever use the Basic authentication with non-SSL connection.
            app.UseBasicAuthentication(new BasicAuthenticationOptions {
                Realm = $"Password: {password}",
                Events = new BasicAuthenticationEvents {
                    OnSignIn = c =>
                    {
                        if (c.Password == password)
                        {
                            var claims = new[] { new Claim(ClaimsIdentity.DefaultNameClaimType, c.UserName) };
                            var identity = new ClaimsIdentity(claims, c.Options.AuthenticationScheme);
                            c.Principal = new ClaimsPrincipal(identity);
                        }

                        return Task.FromResult(true);
                    }
                }
            });

            app.UseMvc();
            
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sample Movie DB");
            });         

        }
    }
}
