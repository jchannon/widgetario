namespace Widgetario.StockApi.Model
{
    using System.Threading.Tasks;
    using Dapper;
    using Microsoft.Extensions.Configuration;
    using Npgsql;

    public class StockRepository
    {
        private readonly string connectionString;

        public StockRepository(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("StockDb");
        }

        public async Task<Product> GetProductById(int id)
        {
            using var connection = new NpgsqlConnection(this.connectionString);
            return await connection.QueryFirstOrDefaultAsync<Product>("select * from products where id = @id",
                new { id = id });
        }
    }
}
