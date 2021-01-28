namespace Widgetario.Web.Services
{
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Widgetario.Web.Models;

    public class StockService
    {
        private readonly IConfiguration _config;

        public StockService(IConfiguration config)
        {
            _config = config;
            ApiUrl = _config["StockApi:Url"];
        }

        public string ApiUrl { get; private set; }

        public async Task<ProductStock> GetStock(long productId)
        {
            var httpClient = new HttpClient();
            return await httpClient.GetFromJsonAsync<ProductStock>($"{ApiUrl}/{productId}",
                new JsonSerializerOptions(JsonSerializerDefaults.Web));
        }
    }
}
