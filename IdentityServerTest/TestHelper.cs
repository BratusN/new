using IdentityServerAspNetIdentity;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Http;

namespace IdentityServerTest
{
    public static class TestHelper
    {
        public static IConfigurationRoot _Configuration { get; }

        static TestHelper()
        {
            _Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("identityconfig.json")
                .Build();
        }

        public static string TargetUrl
        {
            get
            {
                return _Configuration["TargetUrl"];
            }
        }

        public static HttpClient CreateClient()
        {
            var target = TargetUrl;
            var webBuilder = WebHost.CreateDefaultBuilder()
                .UseContentRoot(Path.Combine(GetSolutionBasePath(), typeof(Startup).Namespace))
                .UseConfiguration(_Configuration)
                .UseUrls(TargetUrl)
                .UseStartup<Startup>();

            var server = new TestServer(webBuilder);
            var client = server.CreateClient();
            //  client.BaseAddress = new Uri(TargetUrl);

            return client;
        }

        public static string GetSolutionBasePath()
        {
            var appPath = Directory.GetCurrentDirectory();
            var binPosition = appPath.IndexOf("\\bin", StringComparison.Ordinal);
            var basePath = appPath.Remove(binPosition);

            var backslashPosition = basePath.LastIndexOf("\\", StringComparison.Ordinal);
            basePath = basePath.Remove(backslashPosition);
            return basePath;
        }

    }
}
