using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IGenericRepository<T, TId> where T : class
    {
        Task<T> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T,bool>> predicate, CancellationToken cancellationToken = default);
        Task AddAsync(T entity ,CancellationToken cancellationToken = default);
        Task Update(T entity, CancellationToken cancellationToken = default);
        Task Delete(T entity, CancellationToken cancellationToken = default);
    }
}
