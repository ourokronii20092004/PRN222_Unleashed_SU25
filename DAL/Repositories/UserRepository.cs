using DAL.Data;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UnleashedContext _context;

        public UserRepository(UnleashedContext context)
        {
            _context = context;
        }

        public async Task<User> AddAsync(User user)
        {
            ArgumentNullException.ThrowIfNull(user);
            await _context.Users.AddAsync(user);
            return user;
        }

        public async Task DeleteAsync(Guid userId)
        {
            var user = await GetByIdAsync(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
        }

        public async Task<bool> ExistsByUserEmailAsync(string userEmail)
        {
            if (string.IsNullOrWhiteSpace(userEmail)) return false;
            return await _context.Users.AnyAsync(u => u.UserEmail != null && u.UserEmail.ToLower() == userEmail.ToLower());
        }

        public async Task<bool> ExistsByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) return false;
            return await _context.Users.AnyAsync(u => u.UserUsername != null && u.UserUsername.ToLower() == username.ToLower());
        }

        public async Task<bool> ExistsByUserPhoneAsync(string userPhone)
        {
            if (string.IsNullOrWhiteSpace(userPhone)) return false;
            return await _context.Users.AnyAsync(u => u.UserPhone == userPhone);
        }

        public async Task<User?> FindByUserEmailAsync(string userEmail)
        {
            if (string.IsNullOrWhiteSpace(userEmail)) return null;
            return await _context.Users
                .Include(u => u.Role) // Include Role if often needed
                .FirstOrDefaultAsync(u => u.UserEmail != null && u.UserEmail.ToLower() == userEmail.ToLower());
        }

        public async Task<User?> FindByUserGoogleIdAsync(string googleId)
        {
            if (string.IsNullOrWhiteSpace(googleId)) return null;
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserGoogleId == googleId);
        }

        public async Task<User?> FindByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) return null;
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserUsername != null && u.UserUsername.ToLower() == username.ToLower());
        }

        public async Task<User?> FindByUsernameAndPasswordAsync(string username, string password)
        {
            // IMPORTANT: Password should be hashed. This comparison is for plain text passwords.
            // PHAT, PLS CHANGE
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password)) return null;
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserUsername != null && u.UserUsername.ToLower() == username.ToLower() && u.UserPassword == password);
        }

        public async Task<PagedResult<User>> FindByRoleNamesAndSearchAsync(List<string> roleNames, string? searchTerm, int skip, int take)
        {
            var query = _context.Users.Include(u => u.Role).AsQueryable();

            if (roleNames != null && roleNames.Any())
            {
                var lowerRoleNames = roleNames.Select(r => r.ToLower()).ToList();
                query = query.Where(u => u.Role != null && u.Role.RoleName != null && lowerRoleNames.Contains(u.Role.RoleName.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                string lowerSearchTerm = searchTerm.ToLower();
                query = query.Where(u => (u.UserUsername != null && u.UserUsername.ToLower().Contains(lowerSearchTerm)) ||
                                         (u.UserEmail != null && u.UserEmail.ToLower().Contains(lowerSearchTerm)));
            }

            var totalCount = await query.CountAsync();
            var items = await query.Skip(skip).Take(take).ToListAsync();

            return new PagedResult<User> { Items = items, TotalCount = totalCount };
        }


        public async Task<PagedResult<User>> FindByUsernameOrEmailContainingAsync(string? username, string? userEmail, int skip, int take)
        {
            var query = _context.Users.Include(u => u.Role).AsQueryable();

            bool hasUsernameFilter = !string.IsNullOrWhiteSpace(username);
            bool hasEmailFilter = !string.IsNullOrWhiteSpace(userEmail);

            if (hasUsernameFilter && hasEmailFilter)
            {
                string lowerUsername = username!.ToLower();
                string lowerEmail = userEmail!.ToLower();
                query = query.Where(u => (u.UserUsername != null && u.UserUsername.ToLower().Contains(lowerUsername)) ||
                                         (u.UserEmail != null && u.UserEmail.ToLower().Contains(lowerEmail)));
            }
            else if (hasUsernameFilter)
            {
                string lowerUsername = username!.ToLower();
                query = query.Where(u => u.UserUsername != null && u.UserUsername.ToLower().Contains(lowerUsername));
            }
            else if (hasEmailFilter)
            {
                string lowerEmail = userEmail!.ToLower();
                query = query.Where(u => u.UserEmail != null && u.UserEmail.ToLower().Contains(lowerEmail));
            }
            // If both are null/empty, it returns all users (paged)

            var totalCount = await query.CountAsync();
            var items = await query.OrderBy(u => u.UserUsername).Skip(skip).Take(take).ToListAsync();

            return new PagedResult<User> { Items = items, TotalCount = totalCount };
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.Include(u => u.Role).ToListAsync();
        }

        public async Task<User?> GetByIdAsync(Guid userId) // Changed from string to Guid
        {
            return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task UpdateAsync(User user)
        {
            ArgumentNullException.ThrowIfNull(user);
            _context.Users.Update(user);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}