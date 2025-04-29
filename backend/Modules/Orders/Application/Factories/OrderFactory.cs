using Backend.Modules.Orders.Application.DTOs;
using Backend.Modules.Orders.Domain.Entities;
using Backend.Modules.Orders.Domain.Enums;
using Backend.Modules.Products.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Modules.Orders.Application.Factories
{
    public class OrderFactory
    {
        private readonly IProductQueries _productQueries;
        
        public OrderFactory(IProductQueries productQueries)
        {
            _productQueries = productQueries;
        }

        public async Task<Order> Create(CreateOrderDto createOrderDto)
        {
            var orderProducts = await CreateOrderProductsAsync(createOrderDto.Products);

            var order = new Order
            {
                CreatedDate = DateTime.UtcNow,
                OrderStatusId = (int)OrderStatusEnum.Pending,
                Address = createOrderDto.Address,
                Phone = createOrderDto.Phone,
                OrderProducts = orderProducts
            };
            return order;
        }

        private async Task<List<OrderProduct>> CreateOrderProductsAsync(List<CreateOrderProductDto> productDtos)
        {
            var productIds = productDtos.Select(p => p.ProductId).ToList();

            var products = await _productQueries.GetMultipleByIdAsync(productIds);
             var orderProducts = new List<OrderProduct>();

            foreach (var productDto in productDtos)
            {
                var product = products.FirstOrDefault(p => p.Id == productDto.ProductId);

                if (product == null)
                    throw new KeyNotFoundException($"Product with ID {productDto.ProductId} not found.");

                var orderProduct = new OrderProduct
                {
                    ProductId = productDto.ProductId,
                    ProductQuantity = productDto.Quantity,
                    UnitPrice = product.Price
                };

                orderProducts.Add(orderProduct);
            }

            return orderProducts;
        }

    }
}
