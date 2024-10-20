using Dapper;
using Product_Web.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Product_Web.Repo
{
    public class ProductRepository
    {
        private readonly IConfiguration _configuration;

        public ProductRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection SqlConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            var sql = "SELECT * FROM Products";
            using (var connection = SqlConnection())
            {
                return await connection.QueryAsync<Product>(sql);
            }
        }

        public async Task AddProductAsync(Product product)
        {
            var sql = "INSERT INTO Products (ProductName, Description, Created) VALUES (@ProductName, @Description, @Created)";
            product.Created = DateTime.Now; // Set Created date here
            using (var connection = SqlConnection())
            {
                await connection.ExecuteAsync(sql, product);
            }
        }

        public async Task UpdateProductAsync(Product product)
        {
            var sql = "UPDATE Products SET ProductName = @ProductName, Description = @Description WHERE Id = @Id";
            using (var connection = SqlConnection())
            {
                await connection.ExecuteAsync(sql, product);
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            var sql = "DELETE FROM Products WHERE Id = @Id";
            using (var connection = SqlConnection())
            {
                await connection.ExecuteAsync(sql, new { Id = id });
            }
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var sql = "SELECT * FROM Products WHERE Id = @Id";
            using (var connection = SqlConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Product>(sql, new { Id = id });
            }
        }
    }
}
