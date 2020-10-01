using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Debugging;
using Serilog.Formatting.Compact;

namespace NetCoreApiLinux
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var loggerConfiguration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(loggerConfiguration)
                .Enrich.FromLogContext()
                //.WriteTo.Console(new RenderedCompactJsonFormatter())
                //.WriteTo.Debug(outputTemplate: DateTime.Now.ToString())
                //.WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.Seq("http://localhost:5341")
                .CreateLogger();

            SelfLog.Enable(m => {
                // Destination depends on how you wish to collect messages
                Console.WriteLine(m);
            });

            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
