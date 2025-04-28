using Backend.Modules.Orders.Application.DTOs;
using Backend.Modules.Orders.Domain.Entities;

namespace Backend.Modules.Orders.Application.Factories
{
    public class OrderUpdater
    {
        public void Update(Order order, UpdateOrderDto updateOrderDto)
        {
            if (updateOrderDto.OrderStatusId != null)
                order.Status = updateOrderDto.OrderStatusId;

            if (!string.IsNullOrEmpty(updateOrderDto.Address))
                order.Address = updateOrderDto.Address;

            if (!string.IsNullOrEmpty(updateOrderDto.Phone))
                order.Phone = updateOrderDto.Phone;

            if (updateOrderDto.OrderStatusId == OrderStatus.Delivered && order.DeliveredDate == null)
            {
                order.DeliveredDate = DateTime.UtcNow;
                // Acá podrías también llamar a un servicio para descontar stock si querés.
            }
        }
    }
}
