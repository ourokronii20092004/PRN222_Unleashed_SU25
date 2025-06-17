using BLL.Interfaces;
using DAL.Data;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class AccountService : IAccountService
    {
        private  readonly UnleashedContext _context;
        public AccountService(UnleashedContext context)
        {
            _context = context;
        }

        public async Task<bool> AddUser(User user)
        {
            user.UserCreatedAt = DateTime.UtcNow;
            user.UserUpdatedAt = DateTime.UtcNow;
            await _context.Users.AddAsync(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteUser(User user)
        {
            user.IsUserEnabled = false;
            user.UserUpdatedAt = DateTime.UtcNow;
            _context.Users.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> EditUser(User user)
        {
            try
            {
                user.UserUpdatedAt = DateTime.UtcNow;
                _context.Users.Update(user);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ValidationUser(user.UserUsername).Result)
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<List<User>> GetAccountsAsync()
        {
           return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            if (username == null) return null;
                var user = await _context.Users.Include(user => user.Role)
                    .SingleAsync(user => user.UserUsername.ToLower()
                        .Equals(username.ToLower()));
                return user;                    
        }

        public async Task<bool> ValidationUser(string username)
        {
            if(username == null ) return false;
           return await _context.Users.AnyAsync(user => user.UserUsername.ToLower()
                    .Equals(username.ToLower()));
        }
    }
}
