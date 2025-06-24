using DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAO
{
    public static class UnleashedDAO
    {
        public static IServiceCollection ConnectUnleashedDatabase(this IServiceCollection services, IConfiguration configuration)
        => services.AddDbContext<UnleashedContext>(
            options =>
            options.UseSqlServer(
                configuration.GetConnectionString("Cloudflared")));
        

    }
}
