using AutoMapper;
using CoolTool.DataAccess.DbContexts;
using CoolTool.Entity.Identity;
using CoolTool.QueueProvider;
using CoolTool.UserService.Account;
using CoolTool.UserService.AutoMapper;
using CoolTool.UserService.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoolTool.UserService.Api
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
            services.AddAutoMapper(typeof(UserProfile));
            services.AddMvc();
            services.AddDbContext<UserServiceDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
         
            services.AddRabbitMqClient(Configuration.GetSection("RabbitMq"));
           

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
            services.AddTransient<ICompanyService, CompanyService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseHsts();
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
            app.UseMvc();
        }
    }
}
