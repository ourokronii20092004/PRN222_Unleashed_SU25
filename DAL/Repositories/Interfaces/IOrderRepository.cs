using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order, Guid>
    {
        Task<IEnumerable<Order>> GetOrderListByUserId(Guid userId,CancellationToken cancellationToken = default);
    }
}
