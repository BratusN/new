using System.Collections.Generic;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Linq;
using IdentityModel;
using IdentityServer4.Models;
using Newtonsoft.Json;

namespace IdentityServerAspNetIdentity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            GetApiResources();
            var seed = args.Any(x => x == "/seed");
            if (seed) args = args.Except(new[] { "/seed" }).ToArray();
            var host = CreateWebHostBuilder(args).Build();

            var config = host.Services.GetRequiredService<IConfiguration>();
            var connectionString = config.GetConnectionString("DefaultConnection");
            SeedData.EnsureSeedData(connectionString);

            host.Run();
        }
        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureAppConfiguration(configurationBuilder =>
                {
                    configurationBuilder.AddJsonFile("identityconfig.json");
                })
                .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                    .ReadFrom.Configuration(hostingContext.Configuration));
        }
        public static IEnumerable<ApiResource> GetApiResources()
        {
            var resource = new List<ApiResource>
            {
                new ApiResource("api1", "My API", new[] { JwtClaimTypes.Subject, JwtClaimTypes.Email, "action" })
            };

            var serialized = JsonConvert.SerializeObject(resource);

            return resource;
        }
    }
}
