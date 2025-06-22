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

        public async Task<Role?> GetByIdAsync(int id)
        {
            // FindAsync is suitable for finding by primary key.
            // It first checks the local context, then queries the database if not found locally.
            return await _context.Roles.FindAsync(id);
        }

        public async Task<Role?> FindByNameAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return null;
            }
            // Using ToLower() for case-insensitive comparison.
            // Ensure your database collation also supports case-insensitivity for best performance,
            // or EF Core will do client-side evaluation if ToLower() isn't translated.
            string normalizedRoleName = roleName.ToLower();
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.RoleName != null && r.RoleName.ToLower() == normalizedRoleName);
        }

        public async Task<List<Role>> GetAllAsync()
        {
            return await _context.Roles.OrderBy(r => r.RoleName).ToListAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
