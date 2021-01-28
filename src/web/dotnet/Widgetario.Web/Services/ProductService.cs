namespace Widgetario.Web.Services
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Widgetario.Web.Models;

    public class ProductService
    {
        private readonly IConfiguration _config;

        public ProductService(IConfiguration config)
        {
            _config = config;
            ApiUrl = _config["ProductsApi:Url"];
        }

        public string ApiUrl { get; private set; }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var httpClient = new HttpClient();
            return await httpClient.GetFromJsonAsync<IEnumerable<Product>>(ApiUrl,
                new JsonSerializerOptions(JsonSerializerDefaults.Web));
        }
    }
}
