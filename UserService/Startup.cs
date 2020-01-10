using System;
using CoolTool.DataAccess.DbContexts;
using CoolTool.Entity.Identity;
using CoolTool.UserService.Account;
using CoolTool.UserService.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CoolTool.UserServiceApi
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
            services.AddDbContext<UserServiceDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<Entity.Identity.IdentityUser, Role>(options =>
                    options.Password = new PasswordOptions
                    {
                        RequiredLength = 5,
                        RequireDigit = false,
                        RequireLowercase = false,
                        RequireNonAlphanumeric = false,
                        RequireUppercase = false
                    })
                .AddEntityFrameworkStores<UserServiceDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<ICompanyService,CompanyService>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            //if (Environment.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //    app.UseDatabaseErrorPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //}
            //app.UseStaticFiles();
            //app.UseIdentityServer();
            //app.UseMvcWithDefaultRoute();
        }
    }
}
