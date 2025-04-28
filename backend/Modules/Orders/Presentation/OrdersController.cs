using Microsoft.AspNetCore.Mvc;
using Backend.Modules.Orders.Infrastructure.Persistence;
using Backend.Modules.Orders.Application.Interfaces;

namespace Backend.Modules.Orders.Presentation{

    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly OrdersDbContext _context;
        private readonly IOrderQueries _orderQueries;
        private readonly IOrderCommands _orderCommands;

        public OrdersController(OrdersDbContext context, IOrderQueries orderQueries)
        {
            _context = context;
            _orderQueries = orderQueries;
            _orderCommands = orderCommands;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            try
            {
                var orders = await _orderQueries.GetOrdersAsync();
                if (orders == null || orders.Count == 0)
                    return NotFound("No orders found.");

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            try
            {
                var order = await _orderQueries.FindAsync(id);
                if (order == null)
                    return NotFound($"Order with ID {id} not found.");

                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            try
            {
                if (order == null)
                    return BadRequest("Order is null.");

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(Guid id, [FromBody] Order updatedOrder)
        {
            try
            {
                if (updatedOrder == null || id != updatedOrder.Id)
                    return BadRequest("Order data is invalid.");

                var existingOrder = await _context.Orders.FindAsync(id);
                if (existingOrder == null)
                    return NotFound($"Order with ID {id} not found.");

                // Update fields manually
                existingOrder.Status = updatedOrder.Status;
                existingOrder.TotalAmount = updatedOrder.TotalAmount;
                existingOrder.UpdatedAt = DateTime.UtcNow;
                // (Agregá acá los campos que quieras actualizar)

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            try
            {
                var existingOrder = await _context.Orders.FindAsync(id);
                if (existingOrder == null)
                    return NotFound($"Order with ID {id} not found.");

                _context.Orders.Remove(existingOrder);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
