using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core22SwaggerWebApp.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;

namespace Core22SwaggerWebApp
{
    // https://www.strathweb.com/2016/09/strongly-typed-configuration-in-asp-net-core-without-ioptionst/
    public static class ServiceCollectionExtensions
    {
        public static TConfig ConfigurePoco<TConfig>(
            this IServiceCollection services, 
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //        services.AddDbContext<TodoContext>(opt =>
            //opt.UseInMemoryDatabase("TodoList"));
            //        services.AddMvc();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });


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
            services.ConfigurePoco<CustomSettings>(Configuration.GetSection("CustomSettings"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

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
    }
}
