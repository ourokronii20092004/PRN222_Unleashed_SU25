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
        //Task<Cart> GetCartByIdAsync((Guid userId, int variationId) id);
        //Task<IEnumerable<Cart>> GetAllAsync();
        //Task CreateCartAsync(Cart cart);
        //Task UpdateCartAsync(Cart cart);
        //Task DeleteCartAsync((Guid userId, int variationId) id);
        Task<List<CartDTO>> GetCartItemsAsync(Guid userId);
        Task AddToCartAsync(AddToCartDTO addToCartDTO);
        Task RemoveCartItemAsync(Guid userId, int variationId);
        Task<Guid?> GetUserIdByUserNameAsync(string userName);
        //Task ClearCartAsync(Guid userId);
        //Task<CheckoutDto> CheckoutAsync(Guid userId);
    }
}
