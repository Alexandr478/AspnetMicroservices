using Dapper;
using Discount.API.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Threading.Tasks;

namespace Discount.API.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly string _connectionString;

        public DiscountRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue<string>("DatabaseSettings:ConnectionString");

        }

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var affected =
                await connection.ExecuteAsync
                    ("INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
                            new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });

            if (affected == 0)
                return false;

            return true;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var affected = await connection.ExecuteAsync
                    ("UPDATE Coupon SET ProductName=@ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id",
                            new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount, Id = coupon.Id });

            if (affected == 0)
                return false;

            return true;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var affected = await connection.ExecuteAsync("DELETE FROM Coupon WHERE ProductName = @ProductName",
                new { ProductName = productName });

            if (affected == 0)
                return false;

            return true;
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            Coupon coupon = null;

            try
            {
                coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
                ($"SELECT * FROM {nameof(Coupon)} WHERE {nameof(Coupon.ProductName)} = @{nameof(Coupon.ProductName)}"
                , new { ProductName = productName });
            }
            catch
            {
                return new Coupon { ProductName = "No Discount", Amount = 0, Description = _connectionString };
            }

            if (coupon == null)
            {
                return new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };
            }

            return coupon;
        }

    }
}
