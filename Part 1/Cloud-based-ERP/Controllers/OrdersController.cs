using Cloud_based_ERP.Models;
using Cloud_based_ERP.Repositories;
using Cloud_based_ERP.Services;
using Cloud_based_ERP.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cloud_based_ERP.Controllers
{
    [ApiController]
    [Route("api/v1/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderRepository _repository;
        private readonly ILogisticsService _logistics;

        public OrdersController(OrderRepository repository, ILogisticsService logistics)
        {
            _repository = repository;
            _logistics = logistics;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequest order)
        {
            // Validation
            var errors = OrderValidator.Validate(order);
            if (errors.Any())
                return BadRequest(new { ErrorCode = 1001, Errors = errors });

            // Idempotency
            if (await _repository.OrderExistsAsync(order.RequestId))
                return Conflict(new { ErrorCode = 1002, Message = "Duplicate order detected." });

            // Insert Customer
            var customerId = await _repository.InsertCustomerAsync(order.Customer);

            // Calculate total
            var totalAmount = order.Items.Sum(i => i.Quantity * i.UnitPrice);

            // Insert Order
            var orderId = await _repository.InsertOrderAsync(customerId, order.RequestId, totalAmount);

            // Insert OrderItems
            await _repository.InsertOrderItemsAsync(orderId, order.Items);

            // Fire-and-forget Logistics notification
            _ = _logistics.NotifyOrderAsync(orderId);

            return Ok(new { OrderId = orderId, Message = "Order created successfully." });
        }
    }

}
