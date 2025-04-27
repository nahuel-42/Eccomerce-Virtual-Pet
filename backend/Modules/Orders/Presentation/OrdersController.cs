using Microsoft.AspNetCore.Mvc;
using Backend.Modules.Orders.Infrastructure.Persistence;
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderDbContext _context;
        private readonly IOrderQueries _orderQueries;

        public OrdersController(OrderDbContext context, IOrderQueries orderQueries)
        {
            _context = context;
            _orderQueries = orderQueries;
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
    }
