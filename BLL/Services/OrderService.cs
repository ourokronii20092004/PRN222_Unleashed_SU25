using AutoMapper;
using BLL.Services.Interfaces;
using DAL.DTOs.NotificationDTOs;
using DAL.DTOs.OderDTOs;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly ICartRepository _cartRepo;
        private readonly IMapper _mapper;
        private readonly ICartService _cartService;
        private readonly ILogger<OrderService> _logger;
        private readonly IStockVariationRepository _stockVarRepo;
        private readonly INotificationService _notificationService;
        private readonly IVariationSingleRepository _variationSingleRepo;

        private class OrderNoteItem
        {
            public int VariationId { get; set; }
            public int Quantity { get; set; }
            public decimal? PriceAtPurchase { get; set; }
        }

        public OrderService(
            IOrderRepository orderRepo,
            IMapper mapper,
            ICartRepository cartRepo,
            ICartService cartService,
            ILogger<OrderService> logger,
            IStockVariationRepository stockVarRepo,
            INotificationService notificationService,
            IVariationSingleRepository variationSingleRepo)
        {
            _orderRepo = orderRepo;
            _mapper = mapper;
            _cartRepo = cartRepo;
            _cartService = cartService;
            _logger = logger;
            _stockVarRepo = stockVarRepo;
            _notificationService = notificationService;
            _variationSingleRepo = variationSingleRepo;
        }

        public async Task ApproveOrderAsync(Guid orderId)
        {
            var order = await _orderRepo.GetByIdAsync(orderId);
            
            //new

            if (order != null)
            {
                if (!string.IsNullOrEmpty(order.OrderNote) && order.OrderNote != "Processed")
                {
                    var orderItems = JsonSerializer.Deserialize<List<OrderNoteItem>>(order.OrderNote);
                    if (orderItems != null)
                    {
                        foreach (var item in orderItems)
                        {
                            for (int i = 0; i < item.Quantity; i++)
                            {
                                var newVariationSingle = new VariationSingle
                                {
                                    VariationId = item.VariationId,
                                    IsVariationSingleBought = true,
                                    VariationSingleCode = $"ORD-{orderId.ToString().Substring(0, 4)}-{Guid.NewGuid().ToString().Substring(0, 8)}"
                                };
                                await _variationSingleRepo.AddAsync(newVariationSingle);

                                var orderLink = new OrderVariationSingle
                                {
                                    OrderId = order.OrderId,
                                    VariationSingleId = newVariationSingle.VariationSingleId,
                                    VariationPriceAtPurchase = item.PriceAtPurchase ?? 0
                                };
                                order.OrderVariationSingles.Add(orderLink);
                            }
                        }
                    }
                }

                var varList = order.OrderVariationSingles;
                _logger.LogCritical("ORDER QUANTITY VAR " + order.OrderVariationSingles.Count);

                var variationsToUpdate = new Dictionary<int, int>();
                foreach (var detail in varList)
                {
                    var variationSingle = await _variationSingleRepo.GetByIdAsync(detail.VariationSingleId);
                    if (variationSingle?.VariationId != null)
                    {
                        int variationId = variationSingle.VariationId.Value;
                        if (variationsToUpdate.ContainsKey(variationId))
                        {
                            variationsToUpdate[variationId]++;
                        }
                        else
                        {
                            variationsToUpdate[variationId] = 1;
                        }
                    }
                }

                foreach (var itemToFulfill in variationsToUpdate)
                {
                    int variationId = itemToFulfill.Key;
                    int quantityNeeded = itemToFulfill.Value;

                    var availableStockLocations = await _stockVarRepo.FindByVariationIdAsync(variationId);
                    var locationsWithStock = availableStockLocations.Where(sv => sv.StockQuantity > 0).OrderBy(sv => sv.StockQuantity).ToList();

                    if (!locationsWithStock.Any())
                    {
                        _logger.LogWarning("No stock available for VariationId {VariationId} to fulfill order.", variationId);
                        continue;
                    }

                    foreach (var stockLocation in locationsWithStock)
                    {
                        if (quantityNeeded <= 0)
                        {
                            break;
                        }

                        int quantityToTake = Math.Min(quantityNeeded, stockLocation.StockQuantity.GetValueOrDefault());

                        stockLocation.StockQuantity -= quantityToTake;
                        quantityNeeded -= quantityToTake;

                        await _stockVarRepo.UpdateAsync(stockLocation);

                        _logger.LogCritical("STOCK QUANTITY for VarId {VarId} at StockId {StockId}: {StockQuantity}",
                            variationId, stockLocation.StockId, stockLocation.StockQuantity);

                        if (stockLocation.StockQuantity < 5)
                        {
                            NotificationCreateDTO notif = new NotificationCreateDTO
                            {
                                UsernameSender = "admin123",
                                NotificationTitle = $"LOW STOCK: Var {variationId} / Stock {stockLocation.StockId}",
                                NotificationContent = $"Stock for Variation {variationId} at Stock {stockLocation.StockId} has stock value of {stockLocation.StockQuantity}",
                                IsNotificationDraft = false
                            };
                            await _notificationService.AddNotificationAsync(notif);
                        }
                    }

                    if (quantityNeeded > 0)
                    {
                        _logger.LogError("INSUFFICIENT STOCK for VariationId {VariationId}. Could not fulfill {Shortfall} items.", variationId, quantityNeeded);
                    }
                }

                //end new

                order.OrderStatusId = 2;
                order.OrderNote = "Processed";
                _orderRepo.Update(order);
                await _orderRepo.SaveAsync();
            }
        }

        public async Task CancelOrderAsync(Guid orderId)
        {
            var order = await _orderRepo.GetByIdAsync(orderId);
            if (order != null)
            {
                order.OrderStatusId = 1;
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

            var cartDataForNote = cartItems.Select(item => new
            {
                item.VariationId,
                Quantity = item.CartQuantity ?? 0,
                PriceAtPurchase = item.Variation.VariationPrice
            }).ToList();

            string orderNoteJson = JsonSerializer.Serialize(cartDataForNote);

            var order = new Order
            {
                OrderId = Guid.NewGuid(),
                UserId = userId,
                OrderStatusId = 5,
                OrderDate = DateTimeOffset.Now,
                OrderTotalAmount = totalAmount,
                OrderNote = orderNoteJson
            };

            await _orderRepo.AddAsync(order);
            await _orderRepo.SaveAsync();
            await _cartRepo.RemoveAllAsync(userId);

            _logger.LogInformation("Order {OrderId} created and pending approval.", order.OrderId);

            return (order.OrderId, totalAmount);
        }

        public async Task CreateOrderAsync(OrderDTO dto)
        {
            var order = _mapper.Map<Order>(dto);
            order.OrderDate = DateTimeOffset.Now;
            order.OrderStatusId = 5;
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