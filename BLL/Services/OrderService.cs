    using AutoMapper;
    using BLL.Services.Interfaces;
    using DAL.DTOs.OderDTOs;
    using DAL.Models;
    using DAL.Repositories;
    using DAL.Repositories.Interfaces;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace BLL.Services
    {
        public class OrderService(IOrderRepository orderRepo, IMapper mapper, ICartRepository cartRepo, ICartService cartService, ILogger<OrderService> logger) : IOrderService
        {
            private readonly IOrderRepository _orderRepo = orderRepo;
            private readonly ICartRepository _cartRepo = cartRepo;
            private readonly IMapper _mapper = mapper;
            private readonly ICartService _cartService = cartService;
            private readonly ILogger<OrderService> _logger = logger;

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

            public async Task<(Guid OrderId, decimal? TotalAmount)> ConvertCartToOrderAsync(string username)
            {
                var userId = await _cartService.GetUserIdByUsername(username);
                var cartItems = await _cartRepo.GetAllByUserIdAsync(userId);
                if (cartItems == null || !cartItems.Any())
                {
                    throw new Exception("Cart is empty");
                }

                var totalAmount = cartItems.Sum(item =>
                    item.Variation.VariationPrice * (item.CartQuantity ?? 0));

                var order = new OrderDTO
                {
                    OrderId = Guid.NewGuid(), // Changed from new Guid() to Guid.NewGuid()
                    UserId = userId,
                    OrderStatusId = 5,
                    OrderDate = DateTime.Now,
                    OrderTotalAmount = totalAmount,
                    OrderDetails = cartItems.Select(item => new OrderDetailDTO
                    {
                        VariationSingleId = item.VariationId,
                        VariationPriceAtPurchase = (decimal)item.Variation.VariationPrice,
                        Quantity = item.CartQuantity ?? 0
                    }).ToList()
                };

                await _orderRepo.AddAsync(order);
                await _orderRepo.SaveAsync();
                await _cartRepo.RemoveAllAsync(userId);
                _logger.LogCritical(order.OrderId.ToString());
                return (order.OrderId, totalAmount);
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
