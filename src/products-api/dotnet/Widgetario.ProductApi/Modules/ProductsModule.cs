namespace Widgetario.ProductApi.Modules
{
    using Carter;
    using Carter.Response;
    using Microsoft.Extensions.Logging;
    using Widgetario.ProductApi.Model;

    public class ProductsModule : CarterModule
    {
        public ProductsModule(ProductsRepository productsRepository, ILogger<ProductsModule> logger)
        {
            this.Get("/products", async (req, res) =>
            {
                var products = await productsRepository.GetProducts();
                await res.AsJson(products);
            });
        }
    }
}
