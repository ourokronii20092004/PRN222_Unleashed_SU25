using DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DAL
{
    public class UnleashedContextFactory : IDesignTimeDbContextFactory<UnleashedContext>
    {
        public UnleashedContext CreateDbContext(string[] args)
        {
            // This factory is used by design-time tools like scaffolding and migrations.
            // It builds a configuration object just like the main application does
            // to get the connection string from appsettings.json.

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<UnleashedContext>();

            var connectionString = configuration.GetConnectionString("Cloudflared");

            optionsBuilder.UseSqlServer(connectionString);

            return new UnleashedContext(optionsBuilder.Options);
        }
    }
}