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
        Task<List<Cart>> GetAllByUserIdAsync(Guid userId);
        Task AddOrUpdateAsync(Cart cart);
        Task UpdateQuantityAsync(Guid userId, int variationId, int quantity);
        Task RemoveAsync(Guid userId, int variationId);
        Task RemoveAllAsync(Guid userId);
    }
}
