namespace Widgetario.Web
{
    using Carter;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using OpenTelemetry;
    using OpenTelemetry.Resources;
    using OpenTelemetry.Trace;
    using Widgetario.Web.Services;

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
            services.AddScoped<ProductService>();
            services.AddScoped<StockService>();

            if (Configuration.GetValue<bool>("Tracing:Enabled"))
            {
                services.AddOpenTelemetryTracing(builder =>
                    builder
                        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("Widgetario.Web"))
                        .AddSource("api-load-source")
                        .AddAspNetCoreInstrumentation()
                        .AddSqlClientInstrumentation(sqloptions => sqloptions.SetTextCommandContent = true)
                        .AddHttpClientInstrumentation()
                        .AddJaegerExporter());
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseRouting();
            app.UseEndpoints(builder => builder.MapCarter());
        }
    }
}
