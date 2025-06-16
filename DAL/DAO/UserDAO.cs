using DAL.Data;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAO
{
    internal class UserDAO
    {
        private readonly UnleashedContext _context;

        public UserDAO(UnleashedContext context)
        {
            _context = context;
        }
        public Task<List<User>> GetUsers() => _context.Users.ToListAsync();
        public async Task AddUser(User user) 
        { 
            _context.Users.Add(user); 
            await _context.SaveChangesAsync();
        }
        public async Task UpdateUser(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _context.Users.SingleOrDefaultAsync(
                u => u.UserUsername.ToLower().Equals(username.ToLower()));
        }
        public async Task<bool> IsUserExisted(string username)
        {
            return await _context.Users.AnyAsync(
                u => u.UserUsername.ToLower().Equals(username.ToLower()));
        }

    }
}
