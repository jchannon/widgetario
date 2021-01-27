namespace Widgetario.ProductApi.Model
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Dapper;
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Configuration;
    using Npgsql;

    public class ProductsRepository
    {
        private readonly string connectionString;

        private readonly string dbType;

        public ProductsRepository(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("Mssql");
            this.dbType = configuration.GetValue<string>("Db");
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            switch (this.dbType)
            {
                case "Mssql":
                {
                    using var connection = new SqlConnection(this.connectionString);
                    return await connection.QueryAsync<Product>("select * from products");
                }
                default:
                {
                    using var connection = new NpgsqlConnection(this.connectionString);
                    return await connection.QueryAsync<Product>("select * from products");
                }
            }
        }
    }
}
