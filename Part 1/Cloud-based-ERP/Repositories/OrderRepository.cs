using Cloud_based_ERP.Models;
using Dapper;
using System.Data;

namespace Cloud_based_ERP.Repositories
{
    public class OrderRepository
    {
        private readonly IDbConnection _db;

        public OrderRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<int> InsertCustomerAsync(CustomerDto customer)
        {
            var existingCustomer = await _db.QueryFirstOrDefaultAsync<int?>(
                "SELECT CustomerId FROM Customers WHERE Email=@Email", new { customer.Email });

            if (existingCustomer.HasValue)
                return existingCustomer.Value;

            var sql = @"INSERT INTO Customers (Name, Email) 
                    VALUES (@Name, @Email);
                    SELECT CAST(SCOPE_IDENTITY() as int);";

            return await _db.ExecuteScalarAsync<int>(sql, customer);
        }

        public async Task<bool> OrderExistsAsync(Guid requestId)
        {
            var exists = await _db.QueryFirstOrDefaultAsync<int>(
                "SELECT 1 FROM Orders WHERE RequestId=@RequestId", new { RequestId = requestId });
            return exists == 1;
        }

        public async Task<int> InsertOrderAsync(int customerId, Guid requestId, decimal total)
        {
            var sql = @"INSERT INTO Orders (CustomerId, RequestId, TotalAmount) 
                    VALUES (@CustomerId, @RequestId, @TotalAmount);
                    SELECT CAST(SCOPE_IDENTITY() as int);";

            return await _db.ExecuteScalarAsync<int>(sql, new { CustomerId = customerId, RequestId = requestId, TotalAmount = total });
        }

        public async Task InsertOrderItemsAsync(int orderId, List<OrderItemDto> items)
        {
            var sql = @"INSERT INTO OrderItems (OrderId, ProductName, Quantity, UnitPrice)
                    VALUES (@OrderId, @ProductName, @Quantity, @UnitPrice);";

            foreach (var item in items)
            {
                await _db.ExecuteAsync(sql, new { OrderId = orderId, item.ProductName, item.Quantity, item.UnitPrice });
            }
        }
    }

}
