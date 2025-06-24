using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IColorRepository
    {
        Task AddAsync(Color entity, CancellationToken cancellationToken = default);

        Task Delete(Color entity, CancellationToken cancellationToken = default);

        Task<IEnumerable<Color>> FindAsync(Expression<Func<Color, bool>> predicate, CancellationToken cancellationToken = default);

        Task<List<Color>> GetAllAsync();

        Task<Color?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task Update(Color entity, CancellationToken cancellationToken = default);

        Task<List<Color>> FindAllByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
