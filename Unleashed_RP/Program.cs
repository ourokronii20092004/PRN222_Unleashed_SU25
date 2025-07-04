using BLL.Services;
using BLL.Services.Interfaces;
using BLL.Utilities;
using BLL.Utilities.Interfaces;
using DAL.DAO;
using DAL.Repositories;
using DAL.Repositories.Interfaces;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // --- Database Services ---
        builder.Services.ConnectUnleashedDatabase(builder.Configuration, "Cloudflared");
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

        // --- Image Upload Services ---
        builder.Services.AddHttpClient<IImageUploader, ImgbbImageUploader>();


        // --- Razor Pages Services ---
        builder.Services.AddRazorPages();
        // builder.Services.AddControllers();


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


        // --- Services ---
        builder.Services.AddScoped<IBrandService, BrandService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<INotificationService, NotificationService>();
        builder.Services.AddScoped<ICartService, CartService>();
        builder.Services.AddScoped<IStockService, StockService>();
        builder.Services.AddScoped<IStockTransactionService, StockTransactionService>();
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<IDiscountService, DiscountService>();


        // --- AutoMapper (Copied from MVC project) ---
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        // app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorPages(); 
        // This maps routes for Razor Pages
        // If you added builder.Services.AddControllers(); and have API controllers:
        // app.MapControllers();

        app.Run();
    }
}