namespace Widgetario.Web.Modules
{
    using System.Diagnostics;
    using Carter;
    using Carter.Response;
    using Microsoft.Extensions.Logging;
    using OpenTracing;
    using Widgetario.Web.Models;
    using Widgetario.Web.Services;

    public class HomeModule : CarterModule
    {
        public HomeModule(ProductService productsService, StockService stockService, ITracer tracer,
            ILogger<HomeModule> logger)
        {
            this.Get("/", async (req, res) =>
            {
                var stopwatch = Stopwatch.StartNew();
                logger.LogDebug("Loading products & stock");
                var model = new ProductViewModel();
                using (var loadScope = tracer.BuildSpan("api-load").StartActive())
                {
                    using (var productLoadScope = tracer.BuildSpan("product-api-load").StartActive())
                    {
                        model.Products = await productsService.GetProducts();
                    }

                    foreach (var product in model.Products)
                    {
                        using (var stockLoadScope = tracer.BuildSpan("stock-api-load").StartActive())
                        {
                            var productStock = await stockService.GetStock(product.Id);
                            product.Stock = productStock.Stock;
                        }
                    }
                }

                logger.LogDebug("Products & stock load took: {@Time}ms", stopwatch.Elapsed.TotalMilliseconds);

                await res.AsJson(model);
            });

            this.Get("/error", async (req, res) =>
            {
                await res.AsJson(new ErrorViewModel
                    { RequestId = Activity.Current?.Id ?? req.HttpContext.TraceIdentifier });
            });
        }
    }
}
