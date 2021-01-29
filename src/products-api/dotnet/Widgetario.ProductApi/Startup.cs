namespace Widgetario.ProductApi
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
    using Widgetario.ProductApi.Model;

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

            services.AddTransient<ProductsRepository>();

            if (Configuration.GetValue<bool>("Tracing:Enabled"))
            {
                services.AddOpenTelemetryTracing(builder =>
                    builder
                        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("Widgetario.ProductApi"))
                        .AddAspNetCoreInstrumentation()
                        .AddSqlClientInstrumentation(sqloptions => { sqloptions.SetTextCommandContent = true; })
                        .AddJaegerExporter());
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
