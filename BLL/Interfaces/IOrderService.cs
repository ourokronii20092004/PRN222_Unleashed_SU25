using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IOrderService
    {
        Task<Order> GetOrderByIdAsync(Guid id);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<IEnumerable<Order>> GetOrderListByUserId();
        Task CreateOrder(Order order);
        Task DeleteOrderAsync(Guid id);
        Task UpdateOrderAsync(Guid id, Order order);

    }
}
