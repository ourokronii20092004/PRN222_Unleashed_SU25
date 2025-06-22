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
    public class NotificationRepository : INotificationRepository
    {
        private readonly UnleashedContext _context;
        public NotificationRepository(UnleashedContext context) {
        _context = context;
        }

        public async Task AddAsync(Notification entity, CancellationToken cancellationToken = default)
        {
            await _context.Notifications.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task Delete(Notification entity, CancellationToken cancellationToken = default)
        {
             _context.Notifications.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken) ;
        }

        public async Task<IEnumerable<Notification>> FindAsync(Expression<Func<Notification, bool>> predicate, CancellationToken cancellationToken = default)
        {
           return await _context.Notifications
                .Where(predicate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Notification>> GetAllAsync()
        {
            return await _context.Notifications.ToListAsync();
        }

        public async Task<Notification> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Notifications.FirstOrDefaultAsync(n => n.NotificationId == id);
        }

        public async Task Update(Notification entity, CancellationToken cancellationToken = default)
        {
            _context.Notifications.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
