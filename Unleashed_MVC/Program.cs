using BLL.Interfaces;
using BLL.Services;
using DAL.DAO;
using DAL.Repositories;
using DAL.Repositories.Interfaces;

namespace Unleashed_MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.ConnectUnleashedDatabase(builder.Configuration);
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Add Repository
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<IRoleRepository,RoleRepository>();

            // khai bao services o day
            builder.Services.AddScoped<IBrandService, BrandService>();

            builder.Services.AddScoped<IAccountService, AccountService>();


            //

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
