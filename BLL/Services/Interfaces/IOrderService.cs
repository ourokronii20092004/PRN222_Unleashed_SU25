using DAL.DTOs.OderDTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDTO>> GetOrdersAsync();
        Task<IEnumerable<OrderDTO>> GetOrdersByUserAsync(Guid userId);
        Task<OrderDTO?> GetOrderDetailAsync(Guid id);
        Task CreateOrderAsync(OrderDTO dto);
        Task ApproveOrderAsync(Guid orderId);
        Task CancelOrderAsync(Guid orderId);
        Task<Guid> ConvertCartToOrderAsync(string username);
    }
}
