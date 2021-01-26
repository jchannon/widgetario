namespace Widgetario.StockApi.Modules
{
    using System;
    using Carter;
    using Carter.Request;
    using Carter.Response;
    using EasyCaching.Core;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Widgetario.StockApi.Model;

    public class StockModule : CarterModule
    {
        public StockModule(StockRepository repository, IEasyCachingProvider cache, IConfiguration config,
            ILogger<StockModule> logger)
        {
            this.Get("/stock/{id:int}", async (req, res) =>
            {
                Product product;
                var id = req.RouteValues.As<int>("id");
                if (config.GetValue<bool>("Caching:Enabled"))
                {
                    var cacheProduct = await cache.GetAsync("StockController__products",
                        async () => await repository.GetProductById(id), TimeSpan.FromMinutes(5));
                    product = cacheProduct.Value;
                }
                else
                {
                    product = await repository.GetProductById(id);
                }

                await res.AsJson(product);
            });
        }
    }
}
