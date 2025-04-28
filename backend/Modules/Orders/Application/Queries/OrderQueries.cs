using Backend.Modules.Orders.Application.DTOs;
using Microsoft.EntityFrameworkCore;
using Backend.Modules.Orders.Infrastructure.Persistence;
using Backend.Modules.Orders.Application.Interfaces;

namespace Backend.Modules.Orders.Application.Queries {
        
    public class OrderQueries : IOrderQueries
    {
        private readonly OrdersDbContext _context;
        
        public OrderQueries(OrdersDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrderDto>> GetOrdersAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.Products)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    CreatedDate = o.CreatedDate,
                    DeliveredDate = o.DeliveredDate,
                    Status = o.Status,
                    Phone = o.Phone,
                    Products = o.Products.Select(p => new OrderProductDetailDto
                    {
                        Id = p.ProductId,
                        Name = p.Product.Name,
                        Quantity = p.Quantity,
                        UnitPrice = p.Product.Price
                    }).ToList()
                })
                .ToListAsync();

            return orders;
        }

        public async Task<OrderDto?> GetOrderByIdAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.Products)
                    .ThenInclude(op => op.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                return null;

            return new OrderDto
            {
                Id = order.Id,
                CreatedDate = order.CreatedDate,
                DeliveredDate = order.DeliveredDate,
                Status = order.Status,
                Phone = order.Phone,
                Products = order.Products.Select(p => new OrderProductDetailDto
                {
                    Id = p.ProductId,
                    Name = p.Product.Name,
                    Quantity = p.Quantity,
                    UnitPrice = p.Product.Price
                }).ToList()
            };
        }
    }
}