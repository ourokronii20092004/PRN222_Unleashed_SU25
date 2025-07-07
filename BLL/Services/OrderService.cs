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
            order.OrderId = Guid.NewGuid();
            order.OrderCreatedAt = DateTime.Now;
            await _orderRepo.AddAsync(order);
        }

        public async Task DeleteOrderAsync(Guid id)
        {
            var order = await _orderRepo.GetByIdAsync(id);
            await _orderRepo.Delete(order);
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepo.GetAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(Guid id)
        {
            return await _orderRepo.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Order>> GetOrderListByUserId(Guid userId)
        {
            var orders = await _orderRepo.FindAsync(o =>  o.UserId == userId);
            return orders;
        }

        public async Task UpdateOrderAsync(Guid id, Order order)
        {
            var existingorder = await _orderRepo.GetByIdAsync(id);
            existingorder.OrderUpdatedAt = DateTime.Now;
            await _orderRepo.Update(existingorder);
        }
    }
}
