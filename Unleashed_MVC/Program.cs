using BLL.Services;
using BLL.Services.Interfaces;
using DAL.DAO;
using DAL.Data;
using DAL.Repositories;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Unleashed_MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // --- Database Services ---  
            builder.Services.ConnectUnleashedDatabase(builder.Configuration, "ZeroTierOne");
            /*
             * Configurations:
             * 
             * Cloudflared: This uses the Cloudflared tunnel to connect to the database.
             *              Command: access tcp --hostname sql-server.hault-homelab.io.vn --url 127.0.0.1:4444
             *              
             * ZeroTierOne: This uses ZeroTierOne to connect to the database.
             *              How to: Connect to d5e5fb653773b9bd network using ZeroTierOne client.
             *              
             * Local:       This uses the local database.
             * 
             */


            // --- Add Session services --- 
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Set a timeout for the session
                options.Cookie.HttpOnly = true; // Make the session cookie inaccessible to client-side script
                options.Cookie.IsEssential = true; // Make the session cookie essential for GDPR compliance
            });

            // --- Image Upload Services ---  
            builder.Services.AddHttpClient<BLL.Utilities.Interfaces.IImageUploader, BLL.Utilities.ImgbbImageUploader>();

            // --- MVC Services ---  
            builder.Services.AddControllersWithViews();
            builder.Services.AddControllers();

            // --- Repositories ---  
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddScoped<IBrandRepository, BrandRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
            builder.Services.AddScoped<INotificationUserRepository, NotificationUserRepository>();
            builder.Services.AddScoped<ICartRepository, CartRepository>();
            builder.Services.AddScoped<IStockRepository, StockRepository>();
            builder.Services.AddScoped<IStockVariationRepository, StockVariationRepository>();
            builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
            builder.Services.AddScoped<ITransactionTypeRepository, TransactionTypeRepository>();
            builder.Services.AddScoped<IVariationRepository, VariationRepository>();
            builder.Services.AddScoped<IVariationSingleRepository, VariationSingleRepository>();
            builder.Services.AddScoped<IProductStatusRepository, ProductStatusRepository>();
            builder.Services.AddScoped<IProviderRepository, ProviderRepository>();
            builder.Services.AddScoped<ISizeRepository, SizeRepository>();
            builder.Services.AddScoped<IColorRepository, ColorRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();

            // --- Services ---  
            builder.Services.AddScoped<IBrandService, BrandService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<IStockService, StockService>();
            builder.Services.AddScoped<IStockTransactionService, StockTransactionService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IDiscountService, DiscountService>();

            // --- AutoMapper ---  
            // This scans all assemblies loaded in the current application domain for AutoMapper profiles.  
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            var app = builder.Build();

            // Configure the HTTP request pipeline.  
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.  
                app.UseHsts();
            }

            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Authentication}/{action=Login}");

            app.Run();
        }
    }
}
