using System;
using System.Collections.Generic;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data;

public partial class UnleashedContext : DbContext
{
    public UnleashedContext()
    {
    }

    public UnleashedContext(DbContextOptions<UnleashedContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Color> Colors { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<DiscountStatus> DiscountStatuses { get; set; }

    public virtual DbSet<DiscountType> DiscountTypes { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<NotificationUser> NotificationUsers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<OrderVariationSingle> OrderVariationSingles { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductStatus> ProductStatuses { get; set; }

    public virtual DbSet<Provider> Providers { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<SaleStatus> SaleStatuses { get; set; }

    public virtual DbSet<SaleType> SaleTypes { get; set; }

    public virtual DbSet<ShippingMethod> ShippingMethods { get; set; }

    public virtual DbSet<Size> Sizes { get; set; }

    public virtual DbSet<Stock> Stocks { get; set; }

    public virtual DbSet<StockVariation> StockVariations { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<TransactionType> TransactionTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserDiscount> UserDiscounts { get; set; }

    public virtual DbSet<Variation> Variations { get; set; }

    public virtual DbSet<VariationSingle> VariationSingles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.ToTable("brand");

            entity.Property(e => e.BrandId).HasColumnName("brand_id");
            entity.Property(e => e.BrandCreatedAt).HasColumnName("brand_created_at");
            entity.Property(e => e.BrandDescription)
                .IsUnicode(false)
                .HasColumnName("brand_description");
            entity.Property(e => e.BrandImageUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("brand_image_url");
            entity.Property(e => e.BrandName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("brand_name");
            entity.Property(e => e.BrandUpdatedAt).HasColumnName("brand_updated_at");
            entity.Property(e => e.BrandWebsiteUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("brand_website_url");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.VariationId });

            entity.ToTable("cart");

            entity.Property(e => e.UserId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("user_id");
            entity.Property(e => e.VariationId).HasColumnName("variation_id");
            entity.Property(e => e.CartQuantity).HasColumnName("cart_quantity");

            entity.HasOne(d => d.User).WithMany(p => p.Carts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_cart_user");

            entity.HasOne(d => d.Variation).WithMany(p => p.Carts)
                .HasForeignKey(d => d.VariationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_cart_variation");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("category");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CategoryCreatedAt).HasColumnName("category_created_at");
            entity.Property(e => e.CategoryDescription)
                .IsUnicode(false)
                .HasColumnName("category_description");
            entity.Property(e => e.CategoryImageUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("category_image_url");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("category_name");
            entity.Property(e => e.CategoryUpdatedAt).HasColumnName("category_updated_at");
        });

        modelBuilder.Entity<Color>(entity =>
        {
            entity.ToTable("color");

            entity.Property(e => e.ColorId).HasColumnName("color_id");
            entity.Property(e => e.ColorHexCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("color_hex_code");
            entity.Property(e => e.ColorName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("color_name");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.ToTable("comment");

            entity.Property(e => e.CommentId).HasColumnName("comment_id");
            entity.Property(e => e.CommentContent).HasColumnName("comment_content");
            entity.Property(e => e.CommentCreatedAt).HasColumnName("comment_created_at");
            entity.Property(e => e.CommentUpdatedAt).HasColumnName("comment_updated_at");
            entity.Property(e => e.ParentCommentId).HasColumnName("parent_comment_id");
            entity.Property(e => e.ReviewId).HasColumnName("review_id");

            entity.HasOne(d => d.ParentComment).WithMany(p => p.InverseParentComment)
                .HasForeignKey(d => d.ParentCommentId)
                .HasConstraintName("FK_comment_parent_comment");

            entity.HasOne(d => d.Review).WithMany(p => p.Comments)
                .HasForeignKey(d => d.ReviewId)
                .HasConstraintName("FK_comment_review");

            entity.HasMany(d => d.CommentParents).WithMany(p => p.Comments)
                .UsingEntity<Dictionary<string, object>>(
                    "CommentParent",
                    r => r.HasOne<Comment>().WithMany()
                        .HasForeignKey("CommentParentId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_commentparent_parent"),
                    l => l.HasOne<Comment>().WithMany()
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_commentparent_comment"),
                    j =>
                    {
                        j.HasKey("CommentId", "CommentParentId");
                        j.ToTable("comment_parent");
                        j.IndexerProperty<int>("CommentId").HasColumnName("comment_id");
                        j.IndexerProperty<int>("CommentParentId").HasColumnName("comment_parent_id");
                    });

            entity.HasMany(d => d.Comments).WithMany(p => p.CommentParents)
                .UsingEntity<Dictionary<string, object>>(
                    "CommentParent",
                    r => r.HasOne<Comment>().WithMany()
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_commentparent_comment"),
                    l => l.HasOne<Comment>().WithMany()
                        .HasForeignKey("CommentParentId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_commentparent_parent"),
                    j =>
                    {
                        j.HasKey("CommentId", "CommentParentId");
                        j.ToTable("comment_parent");
                        j.IndexerProperty<int>("CommentId").HasColumnName("comment_id");
                        j.IndexerProperty<int>("CommentParentId").HasColumnName("comment_parent_id");
                    });
        });

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.ToTable("discount");

            entity.HasIndex(e => e.DiscountCode, "UQ_discount_code").IsUnique();

            entity.Property(e => e.DiscountId).HasColumnName("discount_id");
            entity.Property(e => e.DiscountCode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("discount_code");
            entity.Property(e => e.DiscountCreatedAt).HasColumnName("discount_created_at");
            entity.Property(e => e.DiscountDescription)
                .IsUnicode(false)
                .HasColumnName("discount_description");
            entity.Property(e => e.DiscountEndDate).HasColumnName("discount_end_date");
            entity.Property(e => e.DiscountMaximumValue)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("discount_maximum_value");
            entity.Property(e => e.DiscountMinimumOrderValue)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("discount_minimum_order_value");
            entity.Property(e => e.DiscountStartDate).HasColumnName("discount_start_date");
            entity.Property(e => e.DiscountStatusId).HasColumnName("discount_status_id");
            entity.Property(e => e.DiscountTypeId).HasColumnName("discount_type_id");
            entity.Property(e => e.DiscountUpdatedAt).HasColumnName("discount_updated_at");
            entity.Property(e => e.DiscountUsageCount).HasColumnName("discount_usage_count");
            entity.Property(e => e.DiscountUsageLimit).HasColumnName("discount_usage_limit");
            entity.Property(e => e.DiscountValue)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("discount_value");

            entity.HasOne(d => d.DiscountStatus).WithMany(p => p.Discounts)
                .HasForeignKey(d => d.DiscountStatusId)
                .HasConstraintName("FK_discount_status");

            entity.HasOne(d => d.DiscountType).WithMany(p => p.Discounts)
                .HasForeignKey(d => d.DiscountTypeId)
                .HasConstraintName("FK_discount_type");
        });

        modelBuilder.Entity<DiscountStatus>(entity =>
        {
            entity.ToTable("discount_status");

            entity.Property(e => e.DiscountStatusId).HasColumnName("discount_status_id");
            entity.Property(e => e.DiscountStatusName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("discount_status_name");
        });

        modelBuilder.Entity<DiscountType>(entity =>
        {
            entity.ToTable("discount_type");

            entity.Property(e => e.DiscountTypeId).HasColumnName("discount_type_id");
            entity.Property(e => e.DiscountTypeName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("discount_type_name");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.ToTable("notification");

            entity.Property(e => e.NotificationId).HasColumnName("notification_id");
            entity.Property(e => e.IsNotificationDraft).HasColumnName("is_notification_draft");
            entity.Property(e => e.NotificationContent).HasColumnName("notification_content");
            entity.Property(e => e.NotificationCreatedAt).HasColumnName("notification_created_at");
            entity.Property(e => e.NotificationTitle)
                .HasMaxLength(255)
                .HasColumnName("notification_title");
            entity.Property(e => e.NotificationUpdatedAt).HasColumnName("notification_updated_at");
            entity.Property(e => e.UserIdSender)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("user_id_sender");

            entity.HasOne(d => d.UserIdSenderNavigation).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserIdSender)
                .HasConstraintName("FK_notification_user_sender");
        });

        modelBuilder.Entity<NotificationUser>(entity =>
        {
            entity.HasKey(e => new { e.NotificationId, e.UserId });

            entity.ToTable("notification_user");

            entity.Property(e => e.NotificationId).HasColumnName("notification_id");
            entity.Property(e => e.UserId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("user_id");
            entity.Property(e => e.IsNotificationDeleted).HasColumnName("is_notification_deleted");
            entity.Property(e => e.IsNotificationViewed).HasColumnName("is_notification_viewed");

            entity.HasOne(d => d.Notification).WithMany(p => p.NotificationUsers)
                .HasForeignKey(d => d.NotificationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_notificationuser_notification");

            entity.HasOne(d => d.User).WithMany(p => p.NotificationUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_notificationuser_user");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("order");

            entity.Property(e => e.OrderId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("order_id");
            entity.Property(e => e.DiscountId).HasColumnName("discount_id");
            entity.Property(e => e.InchargeEmployeeId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("incharge_employee_id");
            entity.Property(e => e.OrderBillingAddress)
                .HasMaxLength(255)
                .HasColumnName("order_billing_address");
            entity.Property(e => e.OrderCreatedAt).HasColumnName("order_created_at");
            entity.Property(e => e.OrderDate).HasColumnName("order_date");
            entity.Property(e => e.OrderExpectedDeliveryDate).HasColumnName("order_expected_delivery_date");
            entity.Property(e => e.OrderNote).HasColumnName("order_note");
            entity.Property(e => e.OrderStatusId).HasColumnName("order_status_id");
            entity.Property(e => e.OrderTax)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("order_tax");
            entity.Property(e => e.OrderTotalAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("order_total_amount");
            entity.Property(e => e.OrderTrackingNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("order_tracking_number");
            entity.Property(e => e.OrderTransactionReference)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("order_transaction_reference");
            entity.Property(e => e.OrderUpdatedAt).HasColumnName("order_updated_at");
            entity.Property(e => e.PaymentMethodId).HasColumnName("payment_method_id");
            entity.Property(e => e.ShippingMethodId).HasColumnName("shipping_method_id");
            entity.Property(e => e.UserId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("user_id");

            entity.HasOne(d => d.Discount).WithMany(p => p.Orders)
                .HasForeignKey(d => d.DiscountId)
                .HasConstraintName("FK_order_discount");

            entity.HasOne(d => d.InchargeEmployee).WithMany(p => p.OrderInchargeEmployees)
                .HasForeignKey(d => d.InchargeEmployeeId)
                .HasConstraintName("FK_order_incharge_employee");

            entity.HasOne(d => d.OrderStatus).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderStatusId)
                .HasConstraintName("FK_order_status");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PaymentMethodId)
                .HasConstraintName("FK_order_payment_method");

            entity.HasOne(d => d.ShippingMethod).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ShippingMethodId)
                .HasConstraintName("FK_order_shipping_method");

            entity.HasOne(d => d.User).WithMany(p => p.OrderUsers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_order_user");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.ToTable("order_status");

            entity.Property(e => e.OrderStatusId).HasColumnName("order_status_id");
            entity.Property(e => e.OrderStatusName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("order_status_name");
        });

        modelBuilder.Entity<OrderVariationSingle>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.VariationSingleId });

            entity.ToTable("order_variation_single");

            entity.Property(e => e.OrderId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("order_id");
            entity.Property(e => e.VariationSingleId).HasColumnName("variation_single_id");
            entity.Property(e => e.SaleId).HasColumnName("sale_id");
            entity.Property(e => e.VariationPriceAtPurchase)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("variation_price_at_purchase");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderVariationSingles)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ordervarsingle_order");

            entity.HasOne(d => d.VariationSingle).WithMany(p => p.OrderVariationSingles)
                .HasForeignKey(d => d.VariationSingleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ordervarsingle_variation_single");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.ToTable("payment_method");

            entity.Property(e => e.PaymentMethodId).HasColumnName("payment_method_id");
            entity.Property(e => e.PaymentMethodName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("payment_method_name");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("product");

            entity.Property(e => e.ProductId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("product_id");
            entity.Property(e => e.BrandId).HasColumnName("brand_id");
            entity.Property(e => e.ProductCode)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("product_code");
            entity.Property(e => e.ProductCreatedAt).HasColumnName("product_created_at");
            entity.Property(e => e.ProductDescription).HasColumnName("product_description");
            entity.Property(e => e.ProductName)
                .HasMaxLength(255)
                .HasColumnName("product_name");
            entity.Property(e => e.ProductStatusId).HasColumnName("product_status_id");
            entity.Property(e => e.ProductUpdatedAt).HasColumnName("product_updated_at");

            entity.HasOne(d => d.Brand).WithMany(p => p.Products)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("FK_product_brand");

            entity.HasOne(d => d.ProductStatus).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductStatusId)
                .HasConstraintName("FK_product_status");

            entity.HasMany(d => d.Categories).WithMany(p => p.Products)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductCategory",
                    r => r.HasOne<Category>().WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_productcategory_category"),
                    l => l.HasOne<Product>().WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_productcategory_product"),
                    j =>
                    {
                        j.HasKey("ProductId", "CategoryId");
                        j.ToTable("product_category");
                        j.IndexerProperty<string>("ProductId")
                            .HasMaxLength(255)
                            .IsUnicode(false)
                            .HasColumnName("product_id");
                        j.IndexerProperty<int>("CategoryId").HasColumnName("category_id");
                    });
        });

        modelBuilder.Entity<ProductStatus>(entity =>
        {
            entity.ToTable("product_status");

            entity.Property(e => e.ProductStatusId).HasColumnName("product_status_id");
            entity.Property(e => e.ProductStatusName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("product_status_name");
        });

        modelBuilder.Entity<Provider>(entity =>
        {
            entity.ToTable("provider");

            entity.Property(e => e.ProviderId).HasColumnName("provider_id");
            entity.Property(e => e.ProviderAddress)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("provider_address");
            entity.Property(e => e.ProviderCreatedAt).HasColumnName("provider_created_at");
            entity.Property(e => e.ProviderEmail)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("provider_email");
            entity.Property(e => e.ProviderImageUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("provider_image_url");
            entity.Property(e => e.ProviderName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("provider_name");
            entity.Property(e => e.ProviderPhone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("provider_phone");
            entity.Property(e => e.ProviderUpdatedAt).HasColumnName("provider_updated_at");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.ToTable("review");

            entity.Property(e => e.ReviewId).HasColumnName("review_id");
            entity.Property(e => e.OrderId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("order_id");
            entity.Property(e => e.ProductId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("product_id");
            entity.Property(e => e.ReviewRating).HasColumnName("review_rating");
            entity.Property(e => e.UserId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("user_id");

            entity.HasOne(d => d.Order).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_review_order");

            entity.HasOne(d => d.Product).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_review_product");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_review_user");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("role");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.ToTable("sale");

            entity.Property(e => e.SaleId).HasColumnName("sale_id");
            entity.Property(e => e.SaleCreatedAt).HasColumnName("sale_created_at");
            entity.Property(e => e.SaleEndDate).HasColumnName("sale_end_date");
            entity.Property(e => e.SaleStartDate).HasColumnName("sale_start_date");
            entity.Property(e => e.SaleStatusId).HasColumnName("sale_status_id");
            entity.Property(e => e.SaleTypeId).HasColumnName("sale_type_id");
            entity.Property(e => e.SaleUpdatedAt).HasColumnName("sale_updated_at");
            entity.Property(e => e.SaleValue)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("sale_value");

            entity.HasOne(d => d.SaleStatus).WithMany(p => p.Sales)
                .HasForeignKey(d => d.SaleStatusId)
                .HasConstraintName("FK_sale_status");

            entity.HasOne(d => d.SaleType).WithMany(p => p.Sales)
                .HasForeignKey(d => d.SaleTypeId)
                .HasConstraintName("FK_sale_type");

            entity.HasMany(d => d.Products).WithMany(p => p.Sales)
                .UsingEntity<Dictionary<string, object>>(
                    "SaleProduct",
                    r => r.HasOne<Product>().WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_saleproduct_product"),
                    l => l.HasOne<Sale>().WithMany()
                        .HasForeignKey("SaleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_saleproduct_sale"),
                    j =>
                    {
                        j.HasKey("SaleId", "ProductId");
                        j.ToTable("sale_product");
                        j.IndexerProperty<int>("SaleId").HasColumnName("sale_id");
                        j.IndexerProperty<string>("ProductId")
                            .HasMaxLength(255)
                            .IsUnicode(false)
                            .HasColumnName("product_id");
                    });
        });

        modelBuilder.Entity<SaleStatus>(entity =>
        {
            entity.ToTable("sale_status");

            entity.Property(e => e.SaleStatusId).HasColumnName("sale_status_id");
            entity.Property(e => e.SaleStatusName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("sale_status_name");
        });

        modelBuilder.Entity<SaleType>(entity =>
        {
            entity.ToTable("sale_type");

            entity.Property(e => e.SaleTypeId).HasColumnName("sale_type_id");
            entity.Property(e => e.SaleTypeName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("sale_type_name");
        });

        modelBuilder.Entity<ShippingMethod>(entity =>
        {
            entity.ToTable("shipping_method");

            entity.Property(e => e.ShippingMethodId).HasColumnName("shipping_method_id");
            entity.Property(e => e.ShippingMethodName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("shipping_method_name");
        });

        modelBuilder.Entity<Size>(entity =>
        {
            entity.ToTable("size");

            entity.Property(e => e.SizeId).HasColumnName("size_id");
            entity.Property(e => e.SizeName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("size_name");
        });

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.ToTable("stock");

            entity.Property(e => e.StockId).HasColumnName("stock_id");
            entity.Property(e => e.StockAddress)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("stock_address");
            entity.Property(e => e.StockName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("stock_name");
        });

        modelBuilder.Entity<StockVariation>(entity =>
        {
            entity.HasKey(e => new { e.VariationId, e.StockId });

            entity.ToTable("stock_variation");

            entity.Property(e => e.VariationId).HasColumnName("variation_id");
            entity.Property(e => e.StockId).HasColumnName("stock_id");
            entity.Property(e => e.StockQuantity).HasColumnName("stock_quantity");

            entity.HasOne(d => d.Stock).WithMany(p => p.StockVariations)
                .HasForeignKey(d => d.StockId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_stockvariation_stock");

            entity.HasOne(d => d.Variation).WithMany(p => p.StockVariations)
                .HasForeignKey(d => d.VariationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_stockvariation_variation");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.ToTable("transaction");

            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");
            entity.Property(e => e.InchargeEmployeeId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("incharge_employee_id");
            entity.Property(e => e.ProviderId).HasColumnName("provider_id");
            entity.Property(e => e.StockId).HasColumnName("stock_id");
            entity.Property(e => e.TransactionDate).HasColumnName("transaction_date");
            entity.Property(e => e.TransactionProductPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("transaction_product_price");
            entity.Property(e => e.TransactionQuantity).HasColumnName("transaction_quantity");
            entity.Property(e => e.TransactionTypeId).HasColumnName("transaction_type_id");
            entity.Property(e => e.VariationId).HasColumnName("variation_id");

            entity.HasOne(d => d.InchargeEmployee).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.InchargeEmployeeId)
                .HasConstraintName("FK_transaction_incharge_employee");

            entity.HasOne(d => d.Provider).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.ProviderId)
                .HasConstraintName("FK_transaction_provider");

            entity.HasOne(d => d.Stock).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.StockId)
                .HasConstraintName("FK_transaction_stock");

            entity.HasOne(d => d.TransactionType).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.TransactionTypeId)
                .HasConstraintName("FK_transaction_type");

            entity.HasOne(d => d.Variation).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.VariationId)
                .HasConstraintName("FK_transaction_variation");
        });

        modelBuilder.Entity<TransactionType>(entity =>
        {
            entity.ToTable("transaction_type");

            entity.Property(e => e.TransactionTypeId).HasColumnName("transaction_type_id");
            entity.Property(e => e.TransactionTypeName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("transaction_type_name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("user");

            entity.Property(e => e.UserId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("user_id");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.IsUserEnabled).HasColumnName("is_user_enabled");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.UserAddress)
                .HasMaxLength(500)
                .HasColumnName("user_address");
            entity.Property(e => e.UserBirthdate).HasColumnName("user_birthdate");
            entity.Property(e => e.UserCreatedAt).HasColumnName("user_created_at");
            entity.Property(e => e.UserCurrentPaymentMethod)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("user_current_payment_method");
            entity.Property(e => e.UserEmail)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("user_email");
            entity.Property(e => e.UserFullname)
                .HasMaxLength(255)
                .HasColumnName("user_fullname");
            entity.Property(e => e.UserGoogleId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("user_google_id");
            entity.Property(e => e.UserImage)
                .IsUnicode(false)
                .HasColumnName("user_image");
            entity.Property(e => e.UserPassword)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("user_password");
            entity.Property(e => e.UserPhone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("user_phone");
            entity.Property(e => e.UserUpdatedAt).HasColumnName("user_updated_at");
            entity.Property(e => e.UserUsername)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("user_username");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_user_role");
        });

        modelBuilder.Entity<UserDiscount>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.DiscountId });

            entity.ToTable("user_discount");

            entity.Property(e => e.UserId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("user_id");
            entity.Property(e => e.DiscountId).HasColumnName("discount_id");
            entity.Property(e => e.DiscountUsedAt).HasColumnName("discount_used_at");
            entity.Property(e => e.IsDiscountUsed).HasColumnName("is_discount_used");

            entity.HasOne(d => d.Discount).WithMany(p => p.UserDiscounts)
                .HasForeignKey(d => d.DiscountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_userdiscount_discount");

            entity.HasOne(d => d.User).WithMany(p => p.UserDiscounts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_userdiscount_user");
        });

        modelBuilder.Entity<Variation>(entity =>
        {
            entity.ToTable("variation");

            entity.Property(e => e.VariationId).HasColumnName("variation_id");
            entity.Property(e => e.ColorId).HasColumnName("color_id");
            entity.Property(e => e.ProductId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("product_id");
            entity.Property(e => e.SizeId).HasColumnName("size_id");
            entity.Property(e => e.VariationImage)
                .IsUnicode(false)
                .HasColumnName("variation_image");
            entity.Property(e => e.VariationPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("variation_price");

            entity.HasOne(d => d.Color).WithMany(p => p.Variations)
                .HasForeignKey(d => d.ColorId)
                .HasConstraintName("FK_variation_color");

            entity.HasOne(d => d.Product).WithMany(p => p.Variations)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_variation_product");

            entity.HasOne(d => d.Size).WithMany(p => p.Variations)
                .HasForeignKey(d => d.SizeId)
                .HasConstraintName("FK_variation_size");
        });

        modelBuilder.Entity<VariationSingle>(entity =>
        {
            entity.ToTable("variation_single");

            entity.Property(e => e.VariationSingleId).HasColumnName("variation_single_id");
            entity.Property(e => e.IsVariationSingleBought).HasColumnName("is_variation_single_bought");
            entity.Property(e => e.VariationId).HasColumnName("variation_id");
            entity.Property(e => e.VariationSingleCode)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("variation_single_code");

            entity.HasOne(d => d.Variation).WithMany(p => p.VariationSingles)
                .HasForeignKey(d => d.VariationId)
                .HasConstraintName("FK__variation__varia__3A4CA8FD");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
