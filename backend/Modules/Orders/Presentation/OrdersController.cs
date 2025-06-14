using Backend.Modules.Orders.Application.DTOs;
using Backend.Modules.Orders.Application.Interfaces;
using Backend.Modules.Orders.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Modules.Orders.Presentation{

    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly OrdersDbContext _context;
        private readonly IOrderQueries _orderQueries;
        private readonly IOrderCommands _orderCommands;

        public OrdersController(OrdersDbContext context, IOrderQueries orderQueries, IOrderCommands orderCommands)
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
                    orders = new List<OrderDto>();

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("my-orders")]
        [Authorize]
        public async Task<IActionResult> GetOrdersByUser()
        {
            try
            {
                var userIdClaim = User.FindFirst("id")?.Value;
                if (userIdClaim == null)
                    return Unauthorized("User ID not found in token.");

                if (!int.TryParse(userIdClaim, out var userId))
                    return BadRequest("Invalid User ID format.");

                var orders = await _orderQueries.GetOrdersByUserAsync(userId);
                if (orders == null || orders.Count == 0)
                    orders = new List<OrderDto>();

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                var order = await _orderQueries.GetOrderByIdAsync(id);
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
        [Authorize]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            try
            {
                if (createOrderDto == null)
                    return BadRequest("Order data is null.");
                var userIdClaim = User.FindFirst("id")?.Value;

                if (userIdClaim == null)
                    return Unauthorized("User ID not found in token.");

                if (!int.TryParse(userIdClaim, out var userId))
                    return BadRequest("Invalid User ID format.");

                createOrderDto.UserId = userId;

                // Llamada al servicio para crear la orden
                var orderId = await _orderCommands.CreateOrderAsync(createOrderDto);

                return Ok(new OrderResponse{
                    OrderId = orderId,
                    Message = "Order successfully created"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        /*
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderDto updateOrderDto)
        {
            try
            {
                if (updateOrderDto == null)
                    return BadRequest("Order data is null.");

                // Llamada al servicio para actualizar la orden
                await _orderCommands.UpdateOrderStatusAsync(id, updateOrderDto);

                return Ok(new OrderResponse{ Message = "Order successfully updated" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        */
    }
}
