using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core22SwaggerWebApp;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]
//[assembly: ApiConventionType(typeof(CustomApiConventions))]

namespace Core22SwaggerWebApp
{
    //public static class CustomApiConventions
    //{
    //    [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
    //    [ProducesDefaultResponseType]
    //    [ProducesResponseType(200)]
    //    [ProducesResponseType(404)]
    //    public static void Get([ApiConventionNameMatch(ApiConventionNameMatchBehavior.Suffix)][ApiConventionTypeMatch(ApiConventionTypeMatchBehavior.Any)] object code)
    //    { }
    //}

    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Trace()
                .WriteTo.Console()
                .WriteTo.File(@"C:\D\Logs\Core22SwaggerWebApp\log.txt")
                .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog(); // <-- Add this line
    }
}
