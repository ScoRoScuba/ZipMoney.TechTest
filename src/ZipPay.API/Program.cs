using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ZipPay.API
{
    public class Program
    {
        private static IConfiguration _configuration;

        public static void Main(string[] args)
        {
            LoadConfigurtion();

            BuildWebHostBuilder(args).Run();
        }

        private static void LoadConfigurtion()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsetting.json", true, false)
                .AddEnvironmentVariables()
                .Build();
        }

        private static IWebHost BuildWebHostBuilder(params string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, logging) => { logging.AddEventSourceLogger(); })
                .UseConfiguration(_configuration)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseKestrel()
                .UseStartup<StartUp>()
                .Build();
    }
}
