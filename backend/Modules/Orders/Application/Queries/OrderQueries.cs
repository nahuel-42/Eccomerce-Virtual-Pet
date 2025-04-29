using Backend.Modules.Orders.Application.DTOs;
using Backend.Modules.Orders.Application.Interfaces;
using Backend.Modules.Orders.Infrastructure.Persistence;
using Backend.Modules.Orders.Domain.Enums;
using Backend.Modules.Products.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Modules.Orders.Application.Queries {
        
    public class OrderQueries : IOrderQueries
    {
        private readonly OrdersDbContext _context;
        private readonly IProductQueries _productQueries;
        
        public OrderQueries(OrdersDbContext context, IProductQueries productQueries)
        {
            _context = context;
            _productQueries = productQueries;
        }

        public async Task<List<OrderDto>> GetOrdersAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderProducts)
                .Include(o => o.OrderStatus)
                .ToListAsync();

            // Obtener todos los ProductId únicos de las órdenes
            var productIds = orders.SelectMany(o => o.OrderProducts.Select(op => op.ProductId)).Distinct().ToList();

            // Consultar los productos usando IProductQueries
            var products = await _productQueries.GetMultipleByIdAsync(productIds);
            var productDict = products.ToDictionary(p => p.Id, p => p);

            return orders.Select(o => new OrderDto
            {
                Id = o.Id,
                CreatedDate = o.CreatedDate,
                DeliveredDate = o.DeliveredDate,
                Status = o.OrderStatus?.Name ?? Enum.GetName(typeof(OrderStatusEnum), o.OrderStatusId) ?? "Unknown",
                Phone = o.Phone,
                TotalPrice = o.OrderProducts.Sum(p => p.ProductQuantity * p.UnitPrice), 
                Products = o.OrderProducts.Select(p => new OrderProductDto
                {
                    Id = p.ProductId,
                    Name = productDict.ContainsKey(p.ProductId) ? productDict[p.ProductId].Name : "Unknown",
                    Quantity = p.ProductQuantity,
                    UnitPrice = p.UnitPrice
                }).ToList()
            }).ToList();
        }

         public async Task<OrderDto?> GetOrderByIdAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderProducts)
                .Include(o => o.OrderStatus)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                return null;

            // Obtener los ProductId de la orden
            var productIds = order.OrderProducts.Select(op => op.ProductId).Distinct().ToList();

            // Consultar los productos usando IProductQueries
            var products = await _productQueries.GetMultipleByIdAsync(productIds);
            var productDict = products.ToDictionary(p => p.Id, p => p);

            return new OrderDto
            {
                Id = order.Id,
                CreatedDate = order.CreatedDate,
                DeliveredDate = order.DeliveredDate,
                Status = order.OrderStatus?.Name ?? Enum.GetName(typeof(OrderStatusEnum), order.OrderStatusId) ?? "Unknown",
                Phone = order.Phone,
                TotalPrice = order.OrderProducts.Sum(p => p.ProductQuantity * (productDict.ContainsKey(p.ProductId) ? productDict[p.ProductId].Price : 0)), // Calcular TotalPrice
                Products = order.OrderProducts.Select(p => new OrderProductDto
                {
                    Id = p.ProductId,
                    Name = productDict.ContainsKey(p.ProductId) ? productDict[p.ProductId].Name : "Unknown",
                    Quantity = p.ProductQuantity,
                    UnitPrice = p.UnitPrice
                }).ToList()
            };
        }
    }
}