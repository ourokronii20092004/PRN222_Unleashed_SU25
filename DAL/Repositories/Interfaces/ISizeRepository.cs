using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface ISizeRepository
    {
        Task AddAsync(Size entity, CancellationToken cancellationToken = default);
        Task Delete(Size entity, CancellationToken cancellationToken = default);
        Task<IEnumerable<Size>> FindAsync(Expression<Func<Size, bool>> predicate, CancellationToken cancellationToken = default);
        Task<List<Size>> GetAllAsync();
        Task<Size?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task Update(Size entity, CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<List<Size>> FindAllByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
    }
}
