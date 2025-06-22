using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IVariationSingleRepository
    {
        Task<List<VariationSingle>> GetAllAsync();
        Task<VariationSingle?> GetByIdAsync(int id);
        Task<VariationSingle> AddAsync(VariationSingle variationSingle);
        Task UpdateAsync(VariationSingle variationSingle);
        Task DeleteAsync(int id);

        // Corresponds to findByVariationSingleIds(@Param("variationSingleIds") List<Integer> variationSingleIds)
        Task<List<VariationSingle>> FindByVariationSingleIdsAsync(List<int> variationSingleIds);

        Task<int> SaveChangesAsync();
    }
}