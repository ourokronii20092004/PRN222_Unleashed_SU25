using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface ICartRepository 
        //: IGenericRepository<Cart,( Guid,int )>
    {
        Task<List<Cart>> GetCartByUserIdAsync(Guid? userId);
        Task<Cart?> GetCartItemAsync(Guid userId, int variationId);
        Task AddToCartAsync(Cart cart);
        Task UpdateCartItemAsync(Cart cart);
        Task RemoveCartItemAsync(Guid userId, int variationId);
        Task<Guid?> GetUserIdByUserNameAsync(string username);
        //Task ClearCartAsync(Guid userId);
    }
}
