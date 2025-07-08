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
    public class NotificationUserRepository : INotificationUserRepository
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

        public async Task AddRangeAsync(IEnumerable<NotificationUser> notificationUsers, CancellationToken cancellationToken)
        {
            await _context.NotificationUsers.AddRangeAsync(notificationUsers, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task Delete(NotificationUser entity, CancellationToken cancellationToken = default)
        {
            _context.NotificationUsers.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<NotificationUser>> FindAsync(Expression<Func<NotificationUser, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _context.NotificationUsers.Include(nu => nu.Notification)
                .Where(predicate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<NotificationUser>> GetAllAsync()
        {
            return await _context.NotificationUsers.Include(nu => nu.Notification).ToListAsync();
        }

        public async Task<NotificationUser> GetByIdAsync((int, Guid) id, CancellationToken cancellationToken = default)
        {
           var (notificationId, UserId) = id;
            return await _context.NotificationUsers
                .FirstOrDefaultAsync(nu => nu.NotificationId == notificationId && nu.UserId == UserId,cancellationToken);
        }

        public async Task<IEnumerable<NotificationUser>> GetNotificationByUserId(Guid userId)
        {
            return await _context.NotificationUsers
                .Include(nu=>nu.Notification)
                .Where(nu => nu.UserId == userId)
                .ToListAsync(); 
        }

        public async Task<IEnumerable<NotificationUser>> GetNotificationUserByNotificationId(int NotificationId)
        {
            return await _context.NotificationUsers
                 .Where(nu => nu.NotificationId == NotificationId)             
                 .ToListAsync();
        }

        public async Task Update(NotificationUser entity, CancellationToken cancellationToken = default)
        {
            _context.NotificationUsers.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateRangeAsync(IEnumerable<NotificationUser> notificationUsers, CancellationToken cancellationToken = default)
        {
            _context.NotificationUsers.UpdateRange(notificationUsers);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
