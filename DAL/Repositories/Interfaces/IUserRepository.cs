using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    // Define a simple PagedResult class if you need to return paged data from repository
    // Or handle paging parameters (skip, take) directly in methods
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
    }

    public interface IUserRepository
    {
        Task<User?> FindByUserEmailAsync(string userEmail);
        Task<User?> FindByUsernameAsync(string username);
        Task<User?> FindByUsernameAndPasswordAsync(string username, string password); // Password handling will differ
        Task<User?> FindByUserGoogleIdAsync(string googleId);
        Task<bool> ExistsByUsernameAsync(string username);
        Task<bool> ExistsByUserEmailAsync(string userEmail);
        Task<User?> GetByIdAsync(Guid userId); // Changed from string to Guid
        Task<User> AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(Guid userId);

        // For findByUserUsernameContainingOrUserEmailContaining (Pageable pageable)
        Task<PagedResult<User>> FindByUsernameOrEmailContainingAsync(string? username, string? userEmail, int skip, int take);

        // For findByRolesAndSearch (Pageable pageable)
        Task<PagedResult<User>> FindByRoleNamesAndSearchAsync(List<string> roleNames, string? searchTerm, int skip, int take);

        // For findByRolesOnly (Pageable pageable) - Can be combined with above by making searchTerm optional
        // Task<PagedResult<User>> FindByRoleNamesOnlyAsync(List<string> roleNames, int skip, int take);

        Task<bool> ExistsByUserPhoneAsync(string userPhone);

        Task<List<User>> GetAllAsync(); // Standard
        Task<int> SaveChangesAsync();
    }
}