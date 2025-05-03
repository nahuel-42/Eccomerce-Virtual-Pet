using Backend.Modules.Orders.Application.DTOs;
using Backend.Modules.Orders.Application.Interfaces;
using Backend.Modules.Orders.Infrastructure.Persistence;
using Backend.Modules.Orders.Domain.Enums;
using Backend.Modules.Orders.Domain.Entities;
using Backend.Modules.Products.Application.Interfaces;
using Backend.Modules.Users.Application.Interfaces;
using Backend.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Backend.Modules.Orders.Application.Queries {
        
    public class OrderQueries : IOrderQueries
    {
        private readonly OrdersDbContext _context;
        private readonly IProductQueries _productQueries;
        private readonly IUserQueries _userQueries;

        
        public OrderQueries(OrdersDbContext context, IProductQueries productQueries, IUserQueries userQueries)
        {
            _context = context;
            _productQueries = productQueries;
            _userQueries = userQueries;
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

            var userIds = orders.Select(o => o.UserId).Distinct().ToList();
            var users = await _userQueries.GetMultipleByIdAsync(userIds);

            return orders.Select(o => new OrderDto
            {
                Id = o.Id,
                CreatedDate = o.CreatedDate,
                DeliveredDate = o.DeliveredDate,
                Status = o.OrderStatusId,
                Phone = o.Phone,
                Address = o.Address,
                User = MapUserToDto(o.UserId, users),
                TotalPrice = o.OrderProducts.Sum(p => p.ProductQuantity * p.UnitPrice), 
                Products = MapProductsToDto(o.OrderProducts, productDict)
            }).ToList();
        }

        public async Task<List<OrderDto>> GetOrdersByUserAsync(int userId)
        {
            var orders = await _context.Orders
                .Include(o => o.OrderProducts)
                .Include(o => o.OrderStatus)
                .Where(o => o.UserId == userId)
                .ToListAsync();

            var productIds = orders.SelectMany(o => o.OrderProducts.Select(op => op.ProductId)).Distinct().ToList();

            var products = await _productQueries.GetMultipleByIdAsync(productIds);
            var productDict = products.ToDictionary(p => p.Id, p => p);

            var user = await _userQueries.GetUserByIdAsync(userId);

            return orders.Select(o => new OrderDto
            {
                Id = o.Id,
                CreatedDate = o.CreatedDate,
                DeliveredDate = o.DeliveredDate,
                Status = o.OrderStatusId,
                Phone = o.Phone,
                Address = o.Address,
                User = MapUserToDto(user),
                TotalPrice = o.OrderProducts.Sum(p => p.ProductQuantity * p.UnitPrice), 
                Products = MapProductsToDto(o.OrderProducts, productDict)
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

            var user = await _userQueries.GetUserByIdAsync(order.UserId);

            return new OrderDto
            {
                Id = order.Id,
                CreatedDate = order.CreatedDate,
                DeliveredDate = order.DeliveredDate,
                Status = order.OrderStatusId,
                Phone = order.Phone,
                Address = order.Address,
                User = MapUserToDto(user),
                TotalPrice = order.OrderProducts.Sum(p => p.ProductQuantity * (productDict.ContainsKey(p.ProductId) ? productDict[p.ProductId].Price : 0)), // Calcular TotalPrice
                Products = MapProductsToDto(order.OrderProducts, productDict)
            };
        }

        private OrderUserDto MapUserToDto(int userId, List<UserDto> users)
        {
            if (userId == null)
                return null;
            
            return new OrderUserDto
            {
                Id = userId,
                Name = users.FirstOrDefault(u => u.Id == userId)?.Name,
                Email = users.FirstOrDefault(u => u.Id == userId)?.Email
            };
        }

        private OrderUserDto MapUserToDto(UserDto user)
        {
            if (user == null)
                return null;
            
            return new OrderUserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }

        private List<OrderProductDto> MapProductsToDto(IEnumerable<OrderProduct> orderProducts, Dictionary<int, ProductDto> productDict)
        {
            return orderProducts.Select(p => new OrderProductDto
            {
                Id = p.ProductId,
                Name = productDict.ContainsKey(p.ProductId) ? productDict[p.ProductId].Name : "Unknown",
                Quantity = p.ProductQuantity,
                UnitPrice = p.UnitPrice
            }).ToList();
        }
    }
}