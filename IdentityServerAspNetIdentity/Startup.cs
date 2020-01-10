using CoolTool.Entity.Identity;
using IdentityServer4.Models;
using IdentityServerAspNetIdentity.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using CoolTool.DataAccess.DbContexts;
using IdentityUser = CoolTool.Entity.Identity.IdentityUser;

namespace IdentityServerAspNetIdentity
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<IdentityDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<IdentityUser, Role>()
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1);
            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
                .AddInMemoryIdentityResources(Configuration.GetSection("IdentityResources").Get<List<IdentityResource>>())
                .AddInMemoryApiResources(Configuration.GetSection("ApiResources").Get<List<ApiResource>>())
                .AddInMemoryClients(Configuration.GetSection("IdentityServer:Clients"))
                .AddAspNetIdentity<IdentityUser>()
                .AddOperationalStore(options =>
                {
                    options.RedisConnectionString = Configuration.GetConnectionString("RedisConnection");
                });

            var rsa = new RsaKeyService(Configuration["SigningCredentialFilePath"], TimeSpan.FromDays(30));
            services.AddTransient(provider => rsa);
            builder.AddSigningCredential(rsa.GetKey());

            services.AddAuthentication()
               .AddGoogle("Google", options =>
               {
                   var googleAuth = Configuration.GetSection("GoogleAuth");
                   options.ClientId = googleAuth["Id"];
                   options.ClientSecret = googleAuth["Secret"];
               });
        }
        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            app.UseIdentityServer();
            app.UseMvcWithDefaultRoute();
        }
    }


}