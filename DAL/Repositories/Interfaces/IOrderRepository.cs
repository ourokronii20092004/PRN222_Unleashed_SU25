using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IOrderRepository //: IGenericRepository<Order, Guid>
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<IEnumerable<Order>> GetByUserIdAsync(Guid userId);
        Task<Order?> GetByIdAsync(Guid id);
        Task AddAsync(Order order);
        void Update(Order order);
        Task SaveAsync();
    }
}
