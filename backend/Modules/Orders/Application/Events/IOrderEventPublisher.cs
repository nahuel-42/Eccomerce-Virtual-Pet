using System.Threading.Tasks;
using Backend.Modules.Orders.Domain.Entities;
using Backend.Modules.Orders.Domain.Enums;

namespace Backend.Modules.Orders.Application.Events
{
    public interface IOrderEventPublisher
    {
        Task PublishOrderCreatedAsync(Order order);
    }
}