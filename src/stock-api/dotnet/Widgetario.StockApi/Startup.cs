namespace Widgetario.StockApi
{
    using Carter;
    using EasyCaching.Core;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using OpenTelemetry;
    using OpenTelemetry.Resources;
    using OpenTelemetry.Trace;
    using Widgetario.StockApi.Caching;
    using Widgetario.StockApi.Model;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCarter();

            services.AddTransient<StockRepository>();

            if (Configuration.GetValue<bool>("Tracing:Enabled"))
            {
                services.AddOpenTelemetryTracing(builder =>
                    builder
                        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("Widgetario.StockApi"))
                        .AddAspNetCoreInstrumentation()
                        .AddSqlClientInstrumentation(sqloptions => sqloptions.SetTextCommandContent = true)
                        .AddJaegerExporter());
            }

            if (Configuration.GetValue<bool>("Caching:Enabled"))
            {
                services.AddEasyCaching(options => { options.UseInMemory("default"); });
            }
            else
            {
                services.AddSingleton<IEasyCachingProvider>(new NoopCachingProvider());
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(builder => builder.MapCarter());
        }
    }
}
