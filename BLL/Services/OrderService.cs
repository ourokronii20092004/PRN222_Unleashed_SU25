using BLL.Services.Interfaces;
using DAL.Models;
using DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class OrderService : IOrderService
    {
        public IOrderRepository _orderRepo;
        public OrderService(IOrderRepository orderRepo)
        {
            _orderRepo = orderRepo;
        }
        public async Task CreateOrder(Order order)
        {
            order.OrderCreatedAt = DateTime.Now;
            await _orderRepo.AddAsync(order);
        }

        public async Task DeleteOrderAsync(Guid id)
        {
            var order = await _orderRepo.GetByIdAsync(id);
            await _orderRepo.Delete(order);
        }

        public Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetOrderByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Order>> GetOrderListByUserId()
        {
            throw new NotImplementedException();
        }

        public Task UpdateOrderAsync(Guid id, Order order)
        {
            throw new NotImplementedException();
        }
    }
}
