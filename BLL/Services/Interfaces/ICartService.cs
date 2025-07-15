using DAL.DTOs.CartDTOs;
using DAL.DTOs.UserDTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface ICartService
    {
        Task<Guid> GetUserIdByUsername(string username);
        Task<Dictionary<string, List<CartDTO>>> GetCartByUsernameAsync(string username);
        Task AddToCartAsync(string username, int variationId, int quantity);
        Task UpdateQuantityAsync(string username, int variationId, int quantity);
        Task RemoveFromCartAsync(string username, int variationId);
        Task RemoveAllFromCartAsync(string username);
    }
}
