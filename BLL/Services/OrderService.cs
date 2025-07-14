using AutoMapper;
using BLL.Services.Interfaces;
using DAL.DTOs.OderDTOs;
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
        private readonly IOrderRepository _orderRepo;
        private readonly IMapper _mapper;
        public OrderService(IOrderRepository orderRepo, IMapper mapper)
        {
            _orderRepo = orderRepo;
            _mapper = mapper;
        }

        public async Task ApproveOrderAsync(Guid orderId)
        {
            var order = await _orderRepo.GetByIdAsync(orderId);
            if (order != null)
            {
                order.OrderStatusId = 2; // APPROVED
                _orderRepo.Update(order);
                await _orderRepo.SaveAsync();
            }
        }

        public async Task CancelOrderAsync(Guid orderId)
        {
            var order = await _orderRepo.GetByIdAsync(orderId);
            if (order != null)
            {
                order.OrderStatusId = 1; // CANCELLED
                _orderRepo.Update(order);
                await _orderRepo.SaveAsync();
            }
        }

        public async Task CreateOrderAsync(OrderDTO dto)
        {
            var order = _mapper.Map<Order>(dto);
            order.OrderDate = DateTimeOffset.Now;
            order.OrderStatusId = 5; // Default to PENDING
            await _orderRepo.AddAsync(order);
            await _orderRepo.SaveAsync();
        }

        public async Task<OrderDTO?> GetOrderDetailAsync(Guid id)
        {
            var order = await _orderRepo.GetByIdAsync(id);
            return _mapper.Map<OrderDTO>(order);
        }

        public async Task<IEnumerable<OrderDTO>> GetOrdersAsync()
        {
            var orders = await _orderRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderDTO>>(orders);
        }

        public async Task<IEnumerable<OrderDTO>> GetOrdersByUserAsync(Guid userId)
        {
            var orders = await _orderRepo.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<OrderDTO>>(orders);
        }
    }
}
