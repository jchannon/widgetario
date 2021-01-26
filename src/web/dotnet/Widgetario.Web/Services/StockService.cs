namespace Widgetario.Web.Services
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using RestSharp;
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
            var client = new RestClient(ApiUrl);
            var request = new RestRequest($"{productId}");
            var response = await client.ExecuteGetAsync<ProductStock>(request);
            if (!response.IsSuccessful)
            {
                throw new Exception(
                    $"Service call failed, status: {response.StatusCode}, message: {response.ErrorMessage}");
            }

            return response.Data;
        }
    }
}
