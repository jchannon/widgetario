namespace Widgetario.Web.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using RestSharp;
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
            var client = new RestClient(ApiUrl);
            var request = new RestRequest();
            var response = await client.ExecuteGetAsync<IEnumerable<Product>>(request);
            if (!response.IsSuccessful)
            {
                throw new Exception(
                    $"Service call failed, status: {response.StatusCode}, message: {response.ErrorMessage}");
            }

            return response.Data;
        }
    }
}
