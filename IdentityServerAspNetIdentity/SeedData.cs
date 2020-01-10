using CoolTool.Entity.Identity;
using IdentityModel;
using IdentityServerAspNetIdentity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Claims;
using CoolTool.DataAccess.DbContexts;
using IdentityUser = CoolTool.Entity.Identity.IdentityUser;

namespace IdentityServerAspNetIdentity
{
    public class SeedData
    {
        private const string AdminRoleName = "admin";
        private const string AdminEmail = "admin@cooltool.com";

        public static void EnsureSeedData(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddDbContext<IdentityDbContext>(options =>
               options.UseSqlServer(connectionString));
            services.AddIdentity<IdentityUser, Role>(options =>
                    options.Password = new PasswordOptions
                    {
                        RequiredLength = 5,
                        RequireDigit = false,
                        RequireLowercase = false,
                        RequireNonAlphanumeric = false,
                        RequireUppercase = false
                    })
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();
            using (var serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<IdentityDbContext>();
                    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                    var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

                    AddAdminRole(roleMgr);
                    var admin = AddAdmin(userMgr);
                    userMgr.AddToRoleAsync(admin, AdminRoleName);

                    context.SaveChanges();
                }
            }
        }

        private static void AddAdminRole(RoleManager<Role> roleMgr)
        {
            var adminRole = roleMgr.FindByNameAsync(AdminRoleName).Result;
            if (adminRole == null)
            {
                adminRole = new Role
                {
                    Name = AdminRoleName
                };
                var result = roleMgr.CreateAsync(adminRole).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
            }
        }

        private static IdentityUser AddAdmin(UserManager<IdentityUser> userMgr)
        {
            var admin = userMgr.FindByNameAsync(AdminEmail).Result;
            if (admin == null)
            {
                admin = new IdentityUser
                {
                    UserName = AdminEmail,
                    Email = AdminEmail,
                    EmailConfirmed = true
                };
                var result = userMgr.CreateAsync(admin, "admin").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddClaimsAsync(admin, new Claim[]
                {
                    new Claim(JwtClaimTypes.Name, "Admin CoolTool"),
                    new Claim(JwtClaimTypes.Email, AdminEmail),
                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    new Claim("action", "action1"),
                    new Claim("action", "action2")
                }).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
            }
            return userMgr.FindByNameAsync(AdminEmail).Result;
        }
    }
}
