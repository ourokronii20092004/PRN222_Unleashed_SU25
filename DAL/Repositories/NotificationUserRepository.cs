using DAL.Data;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class NotificationUserRepository : INotificationUserRepositorycs
    {
        private readonly UnleashedContext _context;
        public NotificationUserRepository(UnleashedContext context)
        {
            _context = context;
        }

        public async Task AddAsync(NotificationUser entity, CancellationToken cancellationToken = default)
        {
          await _context.NotificationUsers.AddAsync(entity,cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task Delete(NotificationUser entity, CancellationToken cancellationToken = default)
        {
            _context.NotificationUsers.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<NotificationUser>> FindAsync(Expression<Func<NotificationUser, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _context.NotificationUsers
                .Where(predicate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<NotificationUser>> GetAllAsync()
        {
            return await _context.NotificationUsers .ToListAsync();
        }

        public async Task<NotificationUser> GetByIdAsync((int, Guid) id, CancellationToken cancellationToken = default)
        {
           var (notificationId, UserId) = id;
            return await _context.NotificationUsers
                .FirstOrDefaultAsync(nu => nu.NotificationId == notificationId && nu.UserId == UserId,cancellationToken);
        }

        public async Task Update(NotificationUser entity, CancellationToken cancellationToken = default)
        {
            _context.NotificationUsers.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
