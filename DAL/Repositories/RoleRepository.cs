using DAL.Data;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly UnleashedContext _context;
        public RoleRepository(UnleashedContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Role entity, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            await _context.Roles.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task Delete(Role entity, CancellationToken cancellationToken = default)
        {
            _context.Roles.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Role>> FindAsync(Expression<Func<Role, bool>> predicate, CancellationToken cancellationToken = default)
        {
           return await _context.Roles
                .Where(predicate)
                .ToArrayAsync(cancellationToken);
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await _context.Roles.ToArrayAsync();
        }

        public async Task<Role> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
           return await _context.Roles
                .FindAsync(id, cancellationToken);
        }

        public Task Update(Role entity, CancellationToken cancellationToken = default)
        {
            _context.Roles.Update(entity);
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
