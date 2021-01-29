namespace Widgetario.Web.Modules
{
    using System.Diagnostics;
    using Carter;
    using Carter.Response;
    using Microsoft.Extensions.Logging;
    using Widgetario.Web.Models;
    using Widgetario.Web.Services;

    public class HomeModule : CarterModule
    {
        public HomeModule(ProductService productsService, StockService stockService,
            ILogger<HomeModule> logger)
        {
            this.Get("/", async (req, res) =>
            {
                var stopwatch = Stopwatch.StartNew();
                logger.LogDebug("Loading products & stock");
                var model = new ProductViewModel();
                var activitySource = new ActivitySource("api-load-source");

                using (activitySource.StartActivity("api-load"))
                {
                    using (activitySource.StartActivity("product-api-load"))
                    {
                        model.Products = await productsService.GetProducts();
                    }

                    foreach (var product in model.Products)
                    {
                        using (activitySource.StartActivity("stock-api-load"))
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
