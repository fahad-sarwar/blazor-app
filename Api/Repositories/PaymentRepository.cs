using Api.Models;
using Dapper;
using Microsoft.Data.Sqlite;

namespace Api.Repositories
{
    public class PaymentRepository : RepositoryBase
    {
        public async Task<Payment> CreatePayment(Payment payment)
        {
            var query =
                "INSERT INTO Payment (Amount, PaymentMethod, CardName, CardNumber, Expiry, CVV, CreatedAt) " +
                "VALUES (@amount, @paymentMethod, @cardName, @cardNumber, @expiry, @cvv, @createdAt); " +
                "SELECT last_insert_rowid();";

            await using var conn = new SqliteConnection(ConnectionString);

            var paymentId = await conn.QuerySingleAsync<int>(query, new
            {
                amount = payment.Amount,
                paymentMethod = payment.PaymentMethod,
                cardName = payment.CardName,
                cardNumber = payment.CardNumber,
                expiry = payment.Expiry,
                cvv = payment.CVV,
                createdAt = payment.CreatedAt
            });

            payment.Id = paymentId;
            return payment;
        }

        public async Task<Payment?> GetPayment(int paymentId)
        {
            var query =
                "SELECT Id, Amount, PaymentMethod, CardName, CardNumber, Expiry, CVV, CreatedAt " +
                "FROM Payment " +
                "WHERE Id = @paymentId";

            await using var conn = new SqliteConnection(ConnectionString);

            return await conn.QueryFirstOrDefaultAsync<Payment>(query, new { paymentId });
        }
    }
}