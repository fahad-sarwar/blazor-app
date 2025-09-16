using Api.Models;
using Microsoft.Data.Sqlite;

namespace Api.Repositories
{
    public interface IPaymentRepository
    {
        Task<Payment> CreatePayment(Payment payment);
        Task<Payment?> GetPayment(int paymentId);
    }

    public class PaymentRepository(ILogger<PaymentRepository> logger) : RepositoryBase, IPaymentRepository
    {
        public async Task<Payment> CreatePayment(Payment payment)
        {
            var query = 
                "INSERT INTO Payment (Amount, PaymentMethod, CardName, CardNumber, Expiry, CVV, CreatedAt) " +
                "VALUES (@amount, @paymentMethod, @cardName, @cardNumber, @expiry, @cvv, @createdAt); " +
                "SELECT last_insert_rowid();";

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            try
            {
                conn.Open();

                command.Parameters.AddWithValue("@amount", payment.Amount);
                command.Parameters.AddWithValue("@paymentMethod", payment.PaymentMethod);
                command.Parameters.AddWithValue("@cardName", payment.CardName);
                command.Parameters.AddWithValue("@cardNumber", payment.CardNumber);
                command.Parameters.AddWithValue("@expiry", payment.Expiry);
                command.Parameters.AddWithValue("@cvv", payment.CVV);
                command.Parameters.AddWithValue("@createdAt", payment.CreatedAt);

                var paymentDetailsId = await command.ExecuteScalarAsync();
                payment.Id = Convert.ToInt32(paymentDetailsId);
                return payment;
            }
            finally
            {
                conn.Close();
            }
        }

        public async Task<Payment?> GetPayment(int paymentId)
        {
            Payment? payment = null;

            var query = 
                "SELECT Id, Amount, PaymentMethod, CardName, CardNumber, Expiry, CVV, CreatedAt " +
                "FROM Payment " +
                "WHERE Id = @paymentId";

            await using var conn = new SqliteConnection(ConnectionString);
            await using var command = new SqliteCommand(query, conn);
            try
            {
                conn.Open();

                command.Parameters.AddWithValue("@paymentId", paymentId);

                var reader = await command.ExecuteReaderAsync();

                if (reader.Read())
                {
                    payment = new Payment
                    {
                        Id = reader.GetInt32(0),
                        Amount = reader.GetDouble(1),
                        PaymentMethod = reader.GetString(2),
                        CardName = reader.GetString(3),
                        CardNumber = reader.GetString(4),
                        Expiry = reader.GetString(5),
                        CVV = reader.GetString(6),
                        CreatedAt = reader.GetDateTime(7)
                    };
                }

                return payment;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}