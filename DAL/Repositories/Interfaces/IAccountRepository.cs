using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IAccountRepository : IGenericRepository<User, Guid>
    {
        Task<User> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);        
    }
}
