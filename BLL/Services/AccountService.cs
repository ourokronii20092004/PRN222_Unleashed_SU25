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

        public async Task<bool> AddEmployeeAsync(User user)
        {
            user.IsUserEnabled = true;
            user.Role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == 2);
            user.UserCreatedAt = DateTime.UtcNow;
            user.UserUpdatedAt = DateTime.UtcNow;
            await _context.Users.AddAsync(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddCustomerAsync(User user)
        {
            user.IsUserEnabled = true;
            user.Role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == 3);
            user.UserCreatedAt = DateTime.UtcNow;
            user.UserUpdatedAt = DateTime.UtcNow;
            await _context.Users.AddAsync(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteUserAsync(User user)
        {
            user.IsUserEnabled = false;
            user.UserUpdatedAt = DateTime.UtcNow;
            _context.Users.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> EditUserAsync(User user)
        {
            try
            {
                var _user = await GetUserByUsernameAsync(user.UserUsername);
                if (_user != null)
                {
                    _user.UserFullname = user.UserFullname;
                    _user.IsUserEnabled = user.IsUserEnabled;
                    _user.UserEmail = user.UserEmail;
                    _user.UserBirthdate = user.UserBirthdate;
                    _user.UserPhone = user.UserPhone;
                    _user.UserAddress = user.UserAddress;
                    _user.UserImage = user.UserImage;
                    user.UserUpdatedAt = DateTime.UtcNow;
                    _context.Users.Update(_user);
                    return await _context.SaveChangesAsync() > 0;
                }
                return false;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ValidationUserAsync(user.UserUsername).Result)
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

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            if (username == null) return null;
                      
            var user = await _context.Users
                                     .Include(user => user.Role)
                                     .SingleOrDefaultAsync(user => user.UserUsername.Equals(username));
            return user;
        }

        public async Task<bool> ValidationUserAsync(string username)
        {
            if(username == null ) return false;
           return await _context.Users.AnyAsync(user => user.UserUsername.ToLower()
                    .Equals(username.ToLower()));
        }
    }
}
