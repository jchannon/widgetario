namespace Widgetario.ProductApi.Model
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Dapper;
    using Microsoft.Extensions.Configuration;
    using Npgsql;

    public class ProductsRepository
    {
        private readonly string connectionString;

        public ProductsRepository(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("productsdb");
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            using var connection = new NpgsqlConnection(this.connectionString);
            return await connection.QueryAsync<Product>("select * from products");
        }
    }
}
