using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    internal interface IColorRepository
    {
        Task<List<string>> GetAllColorsAsync();
        Task<string?> GetColorByIdAsync(int id);
        Task<string> AddColorAsync(string color);
        Task UpdateColorAsync(int id, string color);
        Task DeleteColorAsync(int id);
        Task<bool> ExistsByNameAsync(string name, int? excludeColorId = null);
        Task<int> SaveChangesAsync(); 
    }
}
