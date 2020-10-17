using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRManagement;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace HRManagement.EmployeeAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Configuration done for Serilog
            Log.Logger = new LoggerConfiguration()
            //.MinimumLevel.Debug()
            //.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.Debug()
            // Add this line:
            .WriteTo.File(
                @"Log/myappSerilog-${shortdate}.txt",
                fileSizeLimitBytes: 1_000_000,
                rollOnFileSizeLimit: true,
                shared: true,
                flushToDiskInterval: TimeSpan.FromSeconds(1))
            .CreateLogger();
            //Configuration done for Serilog

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureLogging((hostBuilderContext, logger) => {
                logger.AddConfiguration(hostBuilderContext.Configuration.GetSection("Logging"));
                logger.AddDebug();
                logger.AddConsole();
                logger.AddEventSourceLogger();
                logger.AddNLog();
                //logger.AddSerilog();
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
            
    }
}
