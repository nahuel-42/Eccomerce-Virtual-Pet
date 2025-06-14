using Backend.Modules.Orders.Application.DTOs;
using Backend.Modules.Orders.Domain.Entities;
using Backend.Modules.Orders.Domain.Enums;

namespace Backend.Modules.Orders.Application.Factories
{
    public class OrderUpdater
    {
        public void Update(Order order, UpdateOrderDto updateOrderDto)
        {
            if (updateOrderDto.OrderStatusId != null)
                order.OrderStatusId = updateOrderDto.OrderStatusId ?? 0;

            if (!string.IsNullOrEmpty(updateOrderDto.Address))
                order.Address = updateOrderDto.Address;

            if (!string.IsNullOrEmpty(updateOrderDto.Phone))
                order.Phone = updateOrderDto.Phone;

            if (updateOrderDto.OrderStatusId.HasValue &&
                updateOrderDto.OrderStatusId == (int)OrderStatusEnum.Entregado &&
                order.DeliveredDate == null)
            {
                order.DeliveredDate = DateTime.UtcNow;
            }
        }
        
        public void UpdateStatus(Order order, OrderStatusEnum status)
        {
            order.OrderStatusId = (int)status;
        }
    }
}
