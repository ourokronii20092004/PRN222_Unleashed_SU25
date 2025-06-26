using DAL.Data;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UnleashedContext _unleashedContext;
        public UserRepository(UnleashedContext unleashedContext) 
            => _unleashedContext = unleashedContext;
        public async Task AddAsync(User entity, CancellationToken cancellationToken = default)
        {
          await _unleashedContext.Users.AddAsync(entity, cancellationToken);
          await _unleashedContext.SaveChangesAsync(cancellationToken);
        }

        public async Task Delete(User entity, CancellationToken cancellationToken = default)
        {
            _unleashedContext.Users.Remove(entity);
            await _unleashedContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _unleashedContext.Users.ToListAsync();
        }

        public async Task Update(User entity, CancellationToken cancellationToken = default)
        {
            _unleashedContext.Users.Update(entity);
            await _unleashedContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(username);
            return await _unleashedContext.Users
                                          .FirstOrDefaultAsync(u => u.UserUsername == username, cancellationToken);
        }

        public async Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(id);
            return await _unleashedContext.Users
                                          .FirstOrDefaultAsync(u => u.UserId == id,cancellationToken);
        }

        public async Task<IEnumerable<User>> FindAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _unleashedContext.Users
                  .Where(predicate)
                  .ToListAsync(cancellationToken);
        }
    }
}