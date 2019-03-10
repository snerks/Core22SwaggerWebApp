using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Core22SwaggerWebApp.Controllers;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;

namespace Core22SwaggerWebApp
{
    //// https://www.strathweb.com/2016/09/strongly-typed-configuration-in-asp-net-core-without-ioptionst/
    //public static class ServiceCollectionExtensions
    //{
    //    public static TConfig ConfigurePoco<TConfig>(
    //        this IServiceCollection services,
    //        IConfiguration configuration) where TConfig : class, new()
    //    {
    //        if (services == null) throw new ArgumentNullException(nameof(services));
    //        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

    //        var config = new TConfig();
    //        configuration.Bind(config);
    //        services.AddSingleton(config);
    //        return config;
    //    }
    //}

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
            services.AddRouting(options => options.LowercaseUrls = true);

            services
                .AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //        services.AddDbContext<TodoContext>(opt =>
            //opt.UseInMemoryDatabase("TodoList"));
            //        services.AddMvc();

            //services.AddHealthChecks()
            //    .AddCheck("sql", () =>
            //    {
            //        //const string _connectionString = "BOGUS";

            //        //try
            //        //{
            //        //    using (var connection = new SqlConnection(_connectionString))
            //        //    {
            //        //        try
            //        //        {
            //        //            connection.Open();
            //        //        }
            //        //        catch (SqlException)
            //        //        {
            //        //            return HealthCheckResult.Unhealthy();
            //        //        }
            //        //    }
            //        //}
            //        //catch (Exception)
            //        //{
            //        //    return HealthCheckResult.Unhealthy();
            //        //}

            //        //return HealthCheckResult.Healthy();
            //        return HealthCheckResult.Unhealthy();

            //    });

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" }));

            //Log.Logger = new LoggerConfiguration()
            //    .WriteTo.File("log.txt")
            //    .CreateLogger();

            //for (var i = 0; i < 10; ++i)
            //{
            //    Log.Information("Hello, file logger!");
            //}

            //Log.CloseAndFlush();

            // Add functionality to inject IOptions<T>
            services.AddOptions();

            // services.Configure<CurrenciesSettings>(Configuration);
            //var currenciesSettings = new CurrenciesSettings();
            //Configuration.GetSection("Currencies").Bind(currenciesSettings);

            //// Use of Get<>
            //var currenciesSettings = 
            //    Configuration
            //    .GetSection("CustomSettings:Currency")
            //    .Get<CurrenciesSettings>();

            ////var currencySettings = Configuration.GetSection("CustomSettings:Currency:Currencies").Get<List<CurrencySettings>>();
            //currenciesSettings.Currencies = 
            //    Configuration
            //    .GetSection("CustomSettings:Currency:Currencies")
            //    .Get<List<CurrencySettings>>();

            //// Use of IOptions<T>
            //var currenciesSettingsSection = Configuration.GetSection("CustomSettings:Currency");
            //services.Configure<CurrenciesSettings>(currenciesSettingsSection);

            //var customSettingsSection = Configuration.GetSection("CustomSettings");
            //services.Configure<CustomSettings>(customSettingsSection);

            // Use of Poco Binding
            // https://www.strathweb.com/2016/09/strongly-typed-configuration-in-asp-net-core-without-ioptionst/
            //services.ConfigurePoco<CustomSettings>(Configuration.GetSection("CustomSettings"));
            //services.ConfigurePoco<CurrenciesSettings>(Configuration.GetSection("CustomSettings:Currency"));

            ConfigurePoco<CustomSettings>(services, Configuration.GetSection("CustomSettings"));
            ConfigurePoco<CurrenciesSettings>(services, Configuration.GetSection("CustomSettings:Currency"));

            services
                .AddHealthChecks()
                    .AddCheck("culture", () =>
                    {
                        const string RequiredCultureIsoCode = "en-GB";

                        var currentUICulture = System.Threading.Thread.CurrentThread.CurrentUICulture;
                        var currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;

                        var isCurrentUICultureCorrect = currentUICulture.IetfLanguageTag == RequiredCultureIsoCode;
                        var isCurrentCultureCorrect = currentCulture.IetfLanguageTag == RequiredCultureIsoCode;

                        var areCurrentCultureSettingsCorrect = isCurrentUICultureCorrect && isCurrentCultureCorrect;

                        if (areCurrentCultureSettingsCorrect)
                        {
                            return HealthCheckResult.Healthy();
                        }

                        var data = new Dictionary<string, object>
                        {
                            ["RequiredCurrentUICulture"] = RequiredCultureIsoCode,
                            ["RequiredCurrentCulture"] = RequiredCultureIsoCode,

                            ["CurrentUICulture"] = System.Threading.Thread.CurrentThread.CurrentUICulture,
                            ["CurrentCulture"] = System.Threading.Thread.CurrentThread.CurrentCulture,
                        };

                        return HealthCheckResult.Unhealthy(
                            $"Incorrect CurrentCulture Settings",
                            null,
                            data);
                    })
                    .AddCheck("sql", () =>
                    {
                        const string _connectionString = "BOGUS";

                        try
                        {
                            using (var connection = new SqlConnection(_connectionString))
                            {
                                try
                                {
                                    connection.Open();
                                }
                                catch (SqlException ex)
                                {
                                    return HealthCheckResult.Unhealthy($"SqlException at {DateTime.Now}", ex);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            var data = new Dictionary<string, object>
                            {
                                ["CurrentUICulture"] = System.Threading.Thread.CurrentThread.CurrentUICulture,
                                ["CurrentCulture"] = System.Threading.Thread.CurrentThread.CurrentCulture,
                            };

                            return HealthCheckResult.Unhealthy(
                                $"Exception : UICulture = [{System.Threading.Thread.CurrentThread.CurrentUICulture}] at {DateTime.Now}",
                                ex,
                                data);
                        }

                        return HealthCheckResult.Healthy();
                        //return HealthCheckResult.Unhealthy("Sample Description");
                    });

            services.AddHealthChecksUI();

            //.AddUrlGroup(new Uri("http://httpbin.org/status/200"), name: "uri-1")
            //.AddUrlGroup(new Uri("http://httpbin.org/status/200"), name: "uri-2")
            //.AddUrlGroup(new Uri("http://httpbin.org/status/500"), name: "uri-3")
            //.Services
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("en-GB"),
                new CultureInfo("fr"),
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-GB"),
                //DefaultRequestCulture = new RequestCulture("en-US"),

                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,

                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            });

            //app.UseHealthChecks("/healthz", new HealthCheckOptions()
            //{
            //    Predicate = _ => true,
            //    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            //})
            //.UseHealthChecksUI(setup =>
            //{
            //    setup.UIPath = "/show-health-ui"; // this is ui path in your browser
            //    setup.ApiPath = "/health-ui-api"; // the UI ( spa app )  use this path to get information from the store ( this is NOT the healthz path, is internal ui api )
            //});
            //// .UseMvc();

            //app.UseHealthChecks("/working", new HealthCheckOptions()
            //{
            //    Predicate = _ => true,
            //    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            //});

            // Get Status Code only
            //app.UseHealthChecks("/working");

            // Get Status Code and other info as JSON
            app.UseHealthChecks("/working", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            });

            app.UseHealthChecksUI();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        // https://www.strathweb.com/2016/09/strongly-typed-configuration-in-asp-net-core-without-ioptionst/
        public TConfig ConfigurePoco<TConfig>(
            IServiceCollection services,
            IConfiguration configuration) where TConfig : class, new()
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            var config = new TConfig();
            configuration.Bind(config);
            services.AddSingleton(config);
            return config;
        }
    }
}
