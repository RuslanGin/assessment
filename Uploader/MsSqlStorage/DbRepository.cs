using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Shared;
using Shared.Models;

namespace MsSqlStorage
{
    public class DbStorage: IStorage
    {
        private readonly string _connectionString;

        public DbStorage(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionString:MsSqlDb"];
        }

        public async Task Save(Order order)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"INSERT INTO [dbo].[Orders] " +
                    $"([Key], [ArticleCode], [Color], [ColorCode], [Description], [Price], [DiscountPrice], [DeliveredIn], [Q1], [Size]) " +
                    $"values " +
                    $"(@Key, @ArticleCode, @Color, @ColorCode, @Description, @Price, @DiscountPrice, @DeliveredIn, @Q1, @Size)", connection);
                command.Parameters.AddWithValue("@Key", order.Key);
                command.Parameters.AddWithValue("@ArticleCode", order.ArticleCode);
                command.Parameters.AddWithValue("@Color", order.Color);
                command.Parameters.AddWithValue("@ColorCode", order.ColorCode);
                command.Parameters.AddWithValue("@Description", order.Description);
                command.Parameters.AddWithValue("@Price", order.Price);
                command.Parameters.AddWithValue("@DiscountPrice", order.DiscountPrice);
                command.Parameters.AddWithValue("@DeliveredIn", order.DeliveredIn);
                command.Parameters.AddWithValue("@Q1", order.Q1);
                command.Parameters.AddWithValue("@Size", order.Size);
                await command.ExecuteNonQueryAsync();
                connection.Close();
            }
        }
    }
}
