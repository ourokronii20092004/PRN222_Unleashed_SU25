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

            // khai bao repository o day
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<IRoleRepository,RoleRepository>();
            builder.Services.AddScoped<IBrandRepository, BrandRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();


            // khai bao services o day
            builder.Services.AddScoped<IBrandService, BrandService>();
            builder.Services.AddScoped<IAccountService, AccountService>();


            // khai bao auto mapper o day
            builder.Services.AddAutoMapper(typeof(BLL.Mappings.BrandProfile).Assembly);

            // khai bao controllers
            builder.Services.AddControllers();


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
