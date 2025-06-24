using DAL.Data;
using DAL.DTOs.ProductDTOs;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly UnleashedContext _context;

        public ProductRepository(UnleashedContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Product entity, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            await _context.Products.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            // Note: IProductRepository AddAsync returns Task<Product>, adjusting this for consistency if possible later
        }

        public async Task Delete(Product entity, CancellationToken cancellationToken = default) // This is hard delete
        {
            _context.Products.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> ExistsByBrandAsync(int brandId)
        {
            return await _context.Products.AnyAsync(p => p.BrandId == brandId);
        }

        public async Task<IEnumerable<Product>> FindAsync(Expression<Func<Product, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _context.Products
               .Where(predicate)
               .Include(p => p.Brand)
               .Include(p => p.ProductStatus)
               .Include(p => p.Categories)
               .ToArrayAsync(cancellationToken);
        }

        // Adjusted existing GetAllAsync to match IProductRepository's List<Product> and include details
        async Task<List<Product>> IProductRepository.GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.ProductStatus)
                .OrderBy(p => p.ProductName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.ProductStatus)
                .OrderBy(p => p.ProductName)
                .ToArrayAsync();
        }


        async Task<Product?> IProductRepository.GetByIdAsync(Guid id)
        {
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.ProductStatus)
                .Include(p => p.Categories)
                .FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public Task<Product> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }


        public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default) // Made nullable
        {
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.ProductStatus)
                .Include(p => p.Categories)
                .FirstOrDefaultAsync(p => p.BrandId == id, cancellationToken); // Assuming this was by BrandId or a typo
        }

        // Adjusted existing GetProductStatusIdAsync to match interface
        public async Task<ProductStatus?> GetProductStatusByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(productId);
            var product = await _context.Products
                .Include(p => p.ProductStatus)
                .Where(p => p.ProductId == productId)
                .Select(p => p.ProductStatus)
                .FirstOrDefaultAsync(cancellationToken);
            return product;
        }

        [Obsolete("Use GetProductStatusByProductIdAsync instead for clarity and correct return type as per interface.")]
        public async Task<ProductStatus?> GetProductStatusIdAsync(Guid productID, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(productID);
            return await _context.Products
                .Where(p => p.ProductId == productID)
                .Select(p => p.ProductStatus)
                .FirstOrDefaultAsync(cancellationToken);
        }


        public async Task Update(Product entity, CancellationToken cancellationToken = default)
        {
            _context.Products.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
            // Note: IProductRepository UpdateAsync returns Task, adjusting this for consistency if possible later
        }

        // Explicit interface implementation for IProductRepository.AddAsync
        async Task<Product> IProductRepository.AddAsync(Product product)
        {
            ArgumentNullException.ThrowIfNull(product);
            await _context.Products.AddAsync(product);
            // await _context.SaveChangesAsync(); // SaveChanges will be called by SaveChangesAsync or service
            return product;
        }

        // Explicit interface implementation for IProductRepository.UpdateAsync
        async Task IProductRepository.UpdateAsync(Product product)
        {
            ArgumentNullException.ThrowIfNull(product);
            _context.Products.Update(product);
            // await _context.SaveChangesAsync(); // SaveChanges will be called by SaveChangesAsync or service
        }


        public async Task<string?> GetProductNameByVariationIdAsync(int variationId)
        {
            return await _context.Variations
                .Where(v => v.VariationId == variationId && v.Product != null)
                .Select(v => v.Product!.ProductName)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ExistsByCategoryIdAsync(int categoryId)
        {
            return await _context.Products.AnyAsync(p => p.Categories.Any(c => c.CategoryId == categoryId));
        }

        public async Task SoftDeleteProductAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                product.ProductStatusId = null; // Or ID of a "Deleted" / "Archived" status
                product.ProductStatus = null;
                // _context.Products.Update(product); // EF Core tracks changes on fetched entity
                await _context.SaveChangesAsync(); // Keep individual save for this specific action as it's a direct command
            }
        }

        public async Task AddProductCategoryAsync(Guid productId, int categoryId)
        {
            var product = await _context.Products.Include(p => p.Categories).FirstOrDefaultAsync(p => p.ProductId == productId);
            var category = await _context.Categories.FindAsync(categoryId);

            if (product != null && category != null)
            {
                if (!product.Categories.Any(c => c.CategoryId == categoryId))
                {
                    product.Categories.Add(category);
                    await _context.SaveChangesAsync(); // Keep individual save for this specific action
                }
            }
        }

        public async Task<Product?> FindProductWithVariationsAsync(Guid productId)
        {
            return await _context.Products
                .Include(p => p.Variations!)
                    .ThenInclude(v => v.Color)
                .Include(p => p.Variations!)
                    .ThenInclude(v => v.Size)
                .Include(p => p.Brand)
                .Include(p => p.ProductStatus)
                .Include(p => p.Categories)
                .FirstOrDefaultAsync(p => p.ProductId == productId);
        }

        public async Task<(List<ProductSearchResultDTO> Products, int TotalCount)> SearchProductsAsync(string? query, int skip, int take)
        {

            string searchQuery = string.IsNullOrWhiteSpace(query) ? "%" : $"%{query.ToLower()}%";
            var queryParam = new SqlParameter("@query", searchQuery);
            var skipParam = new SqlParameter("@skip", skip);
            var takeParam = new SqlParameter("@take", take);

            // This SQL needs to be adapted to your specific database dialect (e.g., SQL Server vs PostgreSQL for LIMIT/OFFSET)
            // The following is an example for SQL Server. PostgreSQL would use LIMIT @take OFFSET @skip.
            // The subquery for first variation and rating aggregation needs careful construction.
            // This is a conceptual SQL structure. You'll need to refine it.
            string sql = $@"
                WITH ProductRatings AS (
                    SELECT
                        p.product_id,
                        COALESCE(AVG(CAST(r.review_rating AS FLOAT)), 0) AS AverageRating,
                        COUNT(r.review_id) AS TotalRatings
                    FROM product p
                    LEFT JOIN review r ON p.product_id = r.product_id
                    GROUP BY p.product_id
                ),
                FirstVariation AS (
                    SELECT
                        v.product_id,
                        v.variation_id,
                        v.variation_image,
                        v.variation_price,
                        s.size_name,
                        c.color_name,
                        ROW_NUMBER() OVER(PARTITION BY v.product_id ORDER BY v.variation_id ASC) as rn
                    FROM variation v
                    LEFT JOIN size s ON v.size_id = s.size_id
                    LEFT JOIN color c ON v.color_id = c.color_id
                )
                SELECT
                    p.product_id AS ProductId,
                    p.product_name AS ProductName,
                    p.product_code AS ProductCode,
                    p.product_description AS ProductDescription,
                    p.brand_id AS BrandId,
                    b.brand_name AS BrandName,
                    p.product_status_id AS ProductStatusId,
                    ps.product_status_name AS ProductStatusName,
                    fv.variation_id AS FirstVariationId,
                    fv.variation_image AS FirstVariationImage,
                    fv.variation_price AS FirstVariationPrice,
                    fv.size_name AS FirstVariationSizeName,
                    fv.color_name AS FirstVariationColorName,
                    pr.AverageRating,
                    pr.TotalRatings
                FROM product p
                LEFT JOIN brand b ON p.brand_id = b.brand_id
                LEFT JOIN product_status ps ON p.product_status_id = ps.product_status_id
                LEFT JOIN ProductRatings pr ON p.product_id = pr.product_id
                LEFT JOIN FirstVariation fv ON p.product_id = fv.product_id AND fv.rn = 1
                WHERE (
                        LOWER(p.product_name) LIKE @query OR
                        LOWER(p.product_code) LIKE @query OR
                        LOWER(p.product_description) LIKE @query
                      )
                  AND p.product_status_id IS NOT NULL
                ORDER BY p.product_id ASC
                OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY;
            ";

            string countSql = $@"
                SELECT COUNT(DISTINCT p.product_id)
                FROM product p
                WHERE (
                        LOWER(p.product_name) LIKE @query OR
                        LOWER(p.product_code) LIKE @query OR
                        LOWER(p.product_description) LIKE @query
                      )
                  AND p.product_status_id IS NOT NULL;
            ";

            var products = await _context.Set<ProductSearchResultDTO>()
                                         .FromSqlRaw(sql, queryParam, skipParam, takeParam)
                                         .ToListAsync();
            // For count, ensure parameters are correctly passed if using ExecuteSqlInterpolated or similar for scalar
            var totalCount = await _context.Database
                                     .SqlQueryRaw<int>(countSql, queryParam)
                                     .SingleAsync();


            return (products, totalCount);
        }

        public async Task<List<Product>> FindByProductIdInAsync(List<Guid> productIds)
        {
            if (productIds == null || !productIds.Any())
                return new List<Product>();
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.ProductStatus)
                .Where(p => productIds.Contains(p.ProductId))
                .ToListAsync();
        }

        public async Task<Guid?> FindIdByProductCodeAsync(string productCode)
        {
            if (string.IsNullOrWhiteSpace(productCode)) return null;
            return await _context.Products
                .Where(p => p.ProductCode == productCode)
                .Select(p => (Guid?)p.ProductId) // Cast to nullable Guid
                .FirstOrDefaultAsync();
        }

        public async Task<List<Product>> FindProductsInStockAsync()
        {
            return await _context.Products
                .Where(p => p.Variations.Any(v => v.StockVariations.Any(sv => sv.StockQuantity > 0)))
                .Include(p => p.Brand)
                .Include(p => p.ProductStatus)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<Guid>> FindIdsByProductCodesAsync(List<string> productCodes)
        {
            if (productCodes == null || !productCodes.Any())
                return new List<Guid>();
            return await _context.Products
                .Where(p => p.ProductCode != null && productCodes.Contains(p.ProductCode))
                .Select(p => p.ProductId)
                .ToListAsync();
        }

        public async Task<List<Product>> FindAllActiveProductsAsync()
        {
            return await _context.Products
                .Where(p => p.ProductStatusId != null)
                .Include(p => p.Brand)
                .Include(p => p.ProductStatus)
                .ToListAsync();
        }

        public async Task<string?> FindNameByProductCodeAsync(string productCode)
        {
            if (string.IsNullOrWhiteSpace(productCode)) return null;
            return await _context.Products
                .Where(p => p.ProductCode == productCode)
                .Select(p => p.ProductName)
                .FirstOrDefaultAsync();
        }

        public async Task<Dictionary<Guid, List<string>>> GetCategoryNamesMapByProductIdsAsync(List<Guid> productIds)
        {
            if (productIds == null || !productIds.Any())
                return new Dictionary<Guid, List<string>>();

            var productsWithCategories = await _context.Products
                .Where(p => productIds.Contains(p.ProductId))
                .Include(p => p.Categories)
                .Select(p => new
                {
                    p.ProductId,
                    CategoryNames = p.Categories.Select(c => c.CategoryName!).ToList() // Assuming CategoryName is not null
                })
                .ToListAsync();

            return productsWithCategories.ToDictionary(
                pc => pc.ProductId,
                pc => pc.CategoryNames
            );
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }


    }
}