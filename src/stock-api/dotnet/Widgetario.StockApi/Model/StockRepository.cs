namespace Widgetario.StockApi.Model
{
    using System.Threading.Tasks;
    using Dapper;
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Configuration;
    using Npgsql;

    public class StockRepository
    {
        private readonly string connectionString;

        private readonly string dbType;

        public StockRepository(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("Mssql");
            this.dbType = configuration.GetValue<string>("Db");
        }

        public async Task<Product> GetProductById(int id)
        {
            switch (this.dbType)
            {
                case "Mssql":
                {
                    using var connection = new SqlConnection(this.connectionString);
                    return await connection.QueryFirstOrDefaultAsync<Product>("select * from products where id = @id",
                        new { id = id });
                }
                default:
                {
                    using var connection = new NpgsqlConnection(this.connectionString);

                    return await connection.QueryFirstOrDefaultAsync<Product>("select * from products where id = @id",
                        new { id = id });
                }
            }
        }
    }
}
