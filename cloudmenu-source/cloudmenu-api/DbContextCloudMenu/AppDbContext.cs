using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using cloudmenu_api.DbEntitiesCloudMenu;

#nullable disable

namespace cloudmenu_api.DbContextCloudMenu
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<MCategory> MCategories { get; set; }
        public virtual DbSet<MCategoryClass> MCategoryClasses { get; set; }
        public virtual DbSet<MCustomer> MCustomers { get; set; }
        public virtual DbSet<MDiscountPremium> MDiscountPremia { get; set; }
        public virtual DbSet<MEquipment> MEquipments { get; set; }
        public virtual DbSet<MEquipmentStock> MEquipmentStocks { get; set; }
        public virtual DbSet<MMaterial> MMaterials { get; set; }
        public virtual DbSet<MMaterialStock> MMaterialStocks { get; set; }
        public virtual DbSet<MProductAllergy> MProductAllergies { get; set; }
        public virtual DbSet<MProductCategory> MProductCategories { get; set; }
        public virtual DbSet<MProductFood> MProductFoods { get; set; }
        public virtual DbSet<MProductMenu> MProductMenus { get; set; }
        public virtual DbSet<MProductMenuClass> MProductMenuClasses { get; set; }
        public virtual DbSet<MProductMenuItem> MProductMenuItems { get; set; }
        public virtual DbSet<MProductSet> MProductSets { get; set; }
        public virtual DbSet<MProductStock> MProductStocks { get; set; }
        public virtual DbSet<MSeat> MSeats { get; set; }
        public virtual DbSet<MStoreGenre> MStoreGenres { get; set; }
        public virtual DbSet<MStoreInfo> MStoreInfos { get; set; }
        public virtual DbSet<MSystemMenu> MSystemMenus { get; set; }
        public virtual DbSet<MTax> MTaxes { get; set; }
        public virtual DbSet<TEquipmentConsumption> TEquipmentConsumptions { get; set; }
        public virtual DbSet<TEquipmentInventory> TEquipmentInventories { get; set; }
        public virtual DbSet<TEquipmentStoringIssue> TEquipmentStoringIssues { get; set; }
        public virtual DbSet<TMaterialConsumption> TMaterialConsumptions { get; set; }
        public virtual DbSet<TMaterialInventory> TMaterialInventories { get; set; }
        public virtual DbSet<TMaterialStoringIssue> TMaterialStoringIssues { get; set; }
        public virtual DbSet<TMoneyInout> TMoneyInouts { get; set; }
        public virtual DbSet<TOrder> TOrders { get; set; }
        public virtual DbSet<TPayment> TPayments { get; set; }
        public virtual DbSet<TPaymentDiscount> TPaymentDiscounts { get; set; }
        public virtual DbSet<TProductConsumption> TProductConsumptions { get; set; }
        public virtual DbSet<TProductInventory> TProductInventories { get; set; }
        public virtual DbSet<TProductStoringIssue> TProductStoringIssues { get; set; }
        public virtual DbSet<TReception> TReceptions { get; set; }
        public virtual DbSet<VEquipmentInventoryNow> VEquipmentInventoryNows { get; set; }
        public virtual DbSet<VMaterialInventoryNow> VMaterialInventoryNows { get; set; }
        public virtual DbSet<VProductInventoryNow> VProductInventoryNows { get; set; }
        public virtual DbSet<VProductLimitSeteach> VProductLimitSeteaches { get; set; }
        public virtual DbSet<VProductLimitSetsum> VProductLimitSetsums { get; set; }
        public virtual DbSet<VProductMenuItem> VProductMenuItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_0900_ai_ci");

            modelBuilder.Entity<MCategory>(entity =>
            {
                entity.HasKey(e => new { e.StoreNumber, e.CategoryClassNumber, e.CategoryKbn })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

                entity.ToTable("m_category");

                entity.HasComment("区分マスタ")
                    .HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.StoreNumber)
                    .HasMaxLength(6)
                    .HasColumnName("store_number")
                    .IsFixedLength(true)
                    .HasComment("店舗番号");

                entity.Property(e => e.CategoryClassNumber)
                    .HasMaxLength(3)
                    .HasColumnName("category_class_number")
                    .IsFixedLength(true)
                    .HasComment("区分分類");

                entity.Property(e => e.CategoryKbn)
                    .HasMaxLength(3)
                    .HasColumnName("category_kbn")
                    .IsFixedLength(true)
                    .HasComment("区分値");

                entity.Property(e => e.CategoryKbnAbbreviation)
                    .HasMaxLength(50)
                    .HasColumnName("category_kbn_abbreviation")
                    .HasComment("区分値略称");

                entity.Property(e => e.CategoryKbnName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("category_kbn_name")
                    .HasComment("区分値名称");

                entity.Property(e => e.CategoryOptionValues)
                    .HasMaxLength(200)
                    .HasColumnName("category_option_values");

                entity.Property(e => e.DelFlg)
                    .HasMaxLength(1)
                    .HasColumnName("del_flg")
                    .HasDefaultValueSql("'0'")
                    .IsFixedLength(true)
                    .HasComment("削除フラグ");

                entity.Property(e => e.RegistrationDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("registration_datetime")
                    .HasComment("登録日時");

                entity.Property(e => e.RegistrationUser)
                    .HasMaxLength(8)
                    .HasColumnName("registration_user")
                    .HasComment("登録者");

                entity.Property(e => e.UpdateDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("update_datetime")
                    .HasComment("更新日時");

                entity.Property(e => e.UpdateUser)
                    .HasMaxLength(8)
                    .HasColumnName("update_user")
                    .HasComment("更新者");
            });

            modelBuilder.Entity<MCategoryClass>(entity =>
            {
                entity.HasKey(e => new { e.StoreNumber, e.CategoryClassNumber })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("m_category_class");

                entity.HasComment("区分分類マスタ")
                    .HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.StoreNumber)
                    .HasMaxLength(6)
                    .HasColumnName("store_number")
                    .IsFixedLength(true)
                    .HasComment("店舗番号");

                entity.Property(e => e.CategoryClassNumber)
                    .HasMaxLength(3)
                    .HasColumnName("category_class_number")
                    .IsFixedLength(true)
                    .HasComment("区分分類");

                entity.Property(e => e.CategoryClassName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("category_class_name")
                    .HasComment("区分分類名");

                entity.Property(e => e.CategoryClassOptionValues)
                    .HasMaxLength(200)
                    .HasColumnName("category_class_option_values");

                entity.Property(e => e.CategoryDescription)
                    .HasColumnType("text")
                    .HasColumnName("category_description")
                    .HasComment("説明");

                entity.Property(e => e.CategorySystemFlg)
                    .HasMaxLength(1)
                    .HasColumnName("category_system_flg")
                    .HasDefaultValueSql("'0'")
                    .IsFixedLength(true)
                    .HasComment("システム管理フラグ");

                entity.Property(e => e.RegistrationDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("registration_datetime")
                    .HasComment("登録日時");

                entity.Property(e => e.RegistrationUser)
                    .HasMaxLength(8)
                    .HasColumnName("registration_user")
                    .HasComment("登録者");

                entity.Property(e => e.UpdateDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("update_datetime")
                    .HasComment("更新日時");

                entity.Property(e => e.UpdateUser)
                    .HasMaxLength(8)
                    .HasColumnName("update_user")
                    .HasComment("更新者");
            });

            modelBuilder.Entity<MCustomer>(entity =>
            {
                entity.HasKey(e => new { e.StoreNumber, e.CustomerNumber })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("m_customer");

                entity.HasComment("顧客マスタ")
                    .HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.StoreNumber)
                    .HasMaxLength(6)
                    .HasColumnName("store_number")
                    .IsFixedLength(true)
                    .HasComment("店舗番号");

                entity.Property(e => e.CustomerNumber)
                    .HasMaxLength(8)
                    .HasColumnName("customer_number")
                    .IsFixedLength(true)
                    .HasComment("顧客コード");

                entity.Property(e => e.CustomerAddress)
                    .HasMaxLength(255)
                    .HasColumnName("customer_address")
                    .HasComment("住所");

                entity.Property(e => e.CustomerBirthday)
                    .HasMaxLength(8)
                    .HasColumnName("customer_birthday")
                    .IsFixedLength(true)
                    .HasComment("生年月日");

                entity.Property(e => e.CustomerConnection)
                    .HasMaxLength(50)
                    .HasColumnName("customer_connection")
                    .HasComment("関係性");

                entity.Property(e => e.CustomerDelFlg)
                    .HasMaxLength(1)
                    .HasColumnName("customer_del_flg")
                    .HasDefaultValueSql("'0'")
                    .IsFixedLength(true)
                    .HasComment("削除フラグ");

                entity.Property(e => e.CustomerEmail)
                    .HasMaxLength(255)
                    .HasColumnName("customer_email")
                    .HasComment("Email");

                entity.Property(e => e.CustomerKbn)
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasColumnName("customer_kbn")
                    .IsFixedLength(true)
                    .HasComment("顧客区分");

                entity.Property(e => e.CustomerName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("customer_name")
                    .HasComment("顧客名");

                entity.Property(e => e.CustomerRemarks)
                    .HasMaxLength(255)
                    .HasColumnName("customer_remarks")
                    .HasComment("備考");

                entity.Property(e => e.CustomerSex)
                    .HasMaxLength(3)
                    .HasColumnName("customer_sex")
                    .IsFixedLength(true)
                    .HasComment("性別区分");

                entity.Property(e => e.CustomerTel)
                    .HasMaxLength(15)
                    .HasColumnName("customer_tel")
                    .HasComment("電話番号");

                entity.Property(e => e.CustomerZip)
                    .HasMaxLength(7)
                    .HasColumnName("customer_zip")
                    .HasComment("郵便番号");

                entity.Property(e => e.RegistrationDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("registration_datetime")
                    .HasComment("登録日時");

                entity.Property(e => e.RegistrationUser)
                    .HasMaxLength(8)
                    .HasColumnName("registration_user")
                    .HasComment("登録者");

                entity.Property(e => e.UpdateDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("update_datetime")
                    .HasComment("更新日時");

                entity.Property(e => e.UpdateUser)
                    .HasMaxLength(8)
                    .HasColumnName("update_user")
                    .HasComment("更新者");
            });

            modelBuilder.Entity<MDiscountPremium>(entity =>
            {
                entity.HasKey(e => new { e.StoreNumber, e.DiscountPremiumKbn })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("m_discount_premium");

                entity.HasComment("割引割増区分マスタ")
                    .HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.StoreNumber)
                    .HasMaxLength(6)
                    .HasColumnName("store_number")
                    .IsFixedLength(true)
                    .HasComment("店舗番号");

                entity.Property(e => e.DiscountPremiumKbn)
                    .HasMaxLength(3)
                    .HasColumnName("discount_premium_kbn")
                    .IsFixedLength(true)
                    .HasComment("割引割増区分");

                entity.Property(e => e.DiscountPercent)
                    .HasColumnName("discount_percent")
                    .HasDefaultValueSql("'0'")
                    .HasComment("割引");

                entity.Property(e => e.DiscountPremiumDelFlg)
                    .HasMaxLength(1)
                    .HasColumnName("discount_premium_del_flg")
                    .HasDefaultValueSql("'0'")
                    .IsFixedLength(true)
                    .HasComment("削除フラグ");

                entity.Property(e => e.DiscountPremiumName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("discount_premium_name")
                    .IsFixedLength(true)
                    .HasComment("割引割増名");

                entity.Property(e => e.DiscountYen)
                    .HasColumnName("discount_yen")
                    .HasDefaultValueSql("'0'")
                    .HasComment("値引き");

                entity.Property(e => e.PremiumPercent)
                    .HasColumnName("premium_percent")
                    .HasDefaultValueSql("'0'")
                    .HasComment("割増");

                entity.Property(e => e.PremiumYen)
                    .HasColumnName("premium_yen")
                    .HasDefaultValueSql("'0'")
                    .HasComment("値上げ");

                entity.Property(e => e.RegistrationDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("registration_datetime")
                    .HasComment("登録日時");

                entity.Property(e => e.RegistrationUser)
                    .HasMaxLength(8)
                    .HasColumnName("registration_user")
                    .HasComment("登録者");

                entity.Property(e => e.UpdateDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("update_datetime")
                    .HasComment("更新日時");

                entity.Property(e => e.UpdateUser)
                    .HasMaxLength(8)
                    .HasColumnName("update_user")
                    .HasComment("更新者");
            });

            modelBuilder.Entity<MEquipment>(entity =>
            {
                entity.HasKey(e => new { e.StoreNumber, e.EquipmentNumber })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("m_equipment");

                entity.HasComment("備品マスタ")
                    .HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.StoreNumber)
                    .HasMaxLength(6)
                    .HasColumnName("store_number")
                    .IsFixedLength(true)
                    .HasComment("店舗番号");

                entity.Property(e => e.EquipmentNumber)
                    .HasMaxLength(8)
                    .HasColumnName("equipment_number")
                    .IsFixedLength(true)
                    .HasComment("備品番号");

                entity.Property(e => e.DelFlg)
                    .HasMaxLength(1)
                    .HasColumnName("del_flg")
                    .HasDefaultValueSql("'0'")
                    .IsFixedLength(true)
                    .HasComment("削除フラグ");

                entity.Property(e => e.EquipmentName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("equipment_name")
                    .HasComment("備品名");

                entity.Property(e => e.EquipmentTypeKbn)
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasColumnName("equipment_type_kbn")
                    .IsFixedLength(true)
                    .HasComment("備品分類区分");

                entity.Property(e => e.EquipmentUnitKbn)
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasColumnName("equipment_unit_kbn")
                    .IsFixedLength(true)
                    .HasComment("単位区分");

                entity.Property(e => e.RegistrationDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("registration_datetime")
                    .HasComment("登録日時");

                entity.Property(e => e.RegistrationUser)
                    .HasMaxLength(8)
                    .HasColumnName("registration_user")
                    .HasComment("登録者");

                entity.Property(e => e.UpdateDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("update_datetime")
                    .HasComment("更新日時");

                entity.Property(e => e.UpdateUser)
                    .HasMaxLength(8)
                    .HasColumnName("update_user")
                    .HasComment("更新者");
            });

            modelBuilder.Entity<MEquipmentStock>(entity =>
            {
                entity.HasKey(e => new { e.StoreNumber, e.EquipmentNumber })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("m_equipment_stock");

                entity.HasComment("備品在庫管理マスタ")
                    .HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.StoreNumber)
                    .HasMaxLength(6)
                    .HasColumnName("store_number")
                    .IsFixedLength(true)
                    .HasComment("店舗番号");

                entity.Property(e => e.EquipmentNumber)
                    .HasMaxLength(8)
                    .HasColumnName("equipment_number")
                    .IsFixedLength(true)
                    .HasComment("備品番号");

                entity.Property(e => e.EquipmentStockQuantity)
                    .HasColumnName("equipment_stock_quantity")
                    .HasDefaultValueSql("'0'")
                    .HasComment("在庫数");

                entity.Property(e => e.RegistrationDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("registration_datetime")
                    .HasComment("登録日時");

                entity.Property(e => e.RegistrationUser)
                    .HasMaxLength(8)
                    .HasColumnName("registration_user")
                    .HasComment("登録者");

                entity.Property(e => e.UpdateDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("update_datetime")
                    .HasComment("更新日時");

                entity.Property(e => e.UpdateUser)
                    .HasMaxLength(8)
                    .HasColumnName("update_user")
                    .HasComment("更新者");
            });

            modelBuilder.Entity<MMaterial>(entity =>
            {
                entity.HasKey(e => new { e.StoreNumber, e.MaterialNumber })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("m_material");

                entity.HasComment("原材料マスタ")
                    .HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.StoreNumber)
                    .HasMaxLength(6)
                    .HasColumnName("store_number")
                    .IsFixedLength(true)
                    .HasComment("店舗番号");

                entity.Property(e => e.MaterialNumber)
                    .HasMaxLength(8)
                    .HasColumnName("material_number")
                    .IsFixedLength(true)
                    .HasComment("原材料番号");

                entity.Property(e => e.DelFlg)
                    .HasMaxLength(1)
                    .HasColumnName("del_flg")
                    .HasDefaultValueSql("'0'")
                    .IsFixedLength(true)
                    .HasComment("削除フラグ");

                entity.Property(e => e.MaterialName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("material_name")
                    .HasComment("原材料名");

                entity.Property(e => e.MaterialTypeKbn)
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasColumnName("material_type_kbn")
                    .IsFixedLength(true)
                    .HasComment("原材料分類区分");

                entity.Property(e => e.MaterialUnitKbn)
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasColumnName("material_unit_kbn")
                    .IsFixedLength(true)
                    .HasComment("単位区分");

                entity.Property(e => e.RegistrationDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("registration_datetime")
                    .HasComment("登録日時");

                entity.Property(e => e.RegistrationUser)
                    .HasMaxLength(8)
                    .HasColumnName("registration_user")
                    .HasComment("登録者");

                entity.Property(e => e.UpdateDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("update_datetime")
                    .HasComment("更新日時");

                entity.Property(e => e.UpdateUser)
                    .HasMaxLength(8)
                    .HasColumnName("update_user")
                    .HasComment("更新者");
            });

            modelBuilder.Entity<MMaterialStock>(entity =>
            {
                entity.HasKey(e => new { e.StoreNumber, e.MaterialNumber })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("m_material_stock");

                entity.HasComment("原材料在庫管理マスタ")
                    .HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.StoreNumber)
                    .HasMaxLength(6)
                    .HasColumnName("store_number")
                    .IsFixedLength(true)
                    .HasComment("店舗番号");

                entity.Property(e => e.MaterialNumber)
                    .HasMaxLength(8)
                    .HasColumnName("material_number")
                    .IsFixedLength(true)
                    .HasComment("原材料番号");

                entity.Property(e => e.MaterialStockQuantity)
                    .HasColumnName("material_stock_quantity")
                    .HasDefaultValueSql("'0'")
                    .HasComment("在庫数");

                entity.Property(e => e.RegistrationDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("registration_datetime")
                    .HasComment("登録日時");

                entity.Property(e => e.RegistrationUser)
                    .HasMaxLength(8)
                    .HasColumnName("registration_user")
                    .HasComment("登録者");

                entity.Property(e => e.UpdateDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("update_datetime")
                    .HasComment("更新日時");

                entity.Property(e => e.UpdateUser)
                    .HasMaxLength(8)
                    .HasColumnName("update_user")
                    .HasComment("更新者");
            });

            modelBuilder.Entity<MProductAllergy>(entity =>
            {
                entity.HasKey(e => new { e.StoreNumber, e.ProductNumber, e.ProductAllergiesKbn })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

                entity.ToTable("m_product_allergies");

                entity.HasComment("商品マスタ（アレルギー情報）")
                    .HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.StoreNumber)
                    .HasMaxLength(6)
                    .HasColumnName("store_number")
                    .IsFixedLength(true)
                    .HasComment("店舗番号");

                entity.Property(e => e.ProductNumber)
                    .HasMaxLength(8)
                    .HasColumnName("product_number")
                    .IsFixedLength(true)
                    .HasComment("商品番号");

                entity.Property(e => e.ProductAllergiesKbn)
                    .HasMaxLength(3)
                    .HasColumnName("product_allergies_kbn")
                    .IsFixedLength(true)
                    .HasComment("商品アレルギー区分");

                entity.Property(e => e.ProductAllergiesFlg)
                    .HasMaxLength(1)
                    .HasColumnName("product_allergies_flg")
                    .HasDefaultValueSql("'0'")
                    .IsFixedLength(true)
                    .HasComment("商品アレルギーフラグ");
            });

            modelBuilder.Entity<MProductCategory>(entity =>
            {
                entity.HasKey(e => new { e.StoreNumber, e.ProductNumber, e.ProductCategoryKbn })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

                entity.ToTable("m_product_category");

                entity.HasComment("商品マスタ（カテゴリ）")
                    .HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.StoreNumber)
                    .HasMaxLength(6)
                    .HasColumnName("store_number")
                    .IsFixedLength(true)
                    .HasComment("店舗番号");

                entity.Property(e => e.ProductNumber)
                    .HasMaxLength(8)
                    .HasColumnName("product_number")
                    .IsFixedLength(true)
                    .HasComment("商品番号");

                entity.Property(e => e.ProductCategoryKbn)
                    .HasMaxLength(3)
                    .HasColumnName("product_category_kbn")
                    .IsFixedLength(true)
                    .HasComment("商品カテゴリ区分");
            });

            modelBuilder.Entity<MProductFood>(entity =>
            {
                entity.HasKey(e => new { e.StoreNumber, e.ProductNumber })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("m_product_food");

                entity.HasComment("商品マスタ")
                    .HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.StoreNumber)
                    .HasMaxLength(6)
                    .HasColumnName("store_number")
                    .IsFixedLength(true)
                    .HasComment("店舗番号");

                entity.Property(e => e.ProductNumber)
                    .HasMaxLength(8)
                    .HasColumnName("product_number")
                    .IsFixedLength(true)
                    .HasComment("商品番号");

                entity.Property(e => e.DelFlg)
                    .HasMaxLength(1)
                    .HasColumnName("del_flg")
                    .HasDefaultValueSql("'0'")
                    .IsFixedLength(true)
                    .HasComment("削除フラグ");

                entity.Property(e => e.ProductCalorie)
                    .HasColumnName("product_calorie")
                    .HasDefaultValueSql("'0'")
                    .HasComment("カロリー");

                entity.Property(e => e.ProductCookingtime)
                    .HasColumnName("product_cookingtime")
                    .HasDefaultValueSql("'0'")
                    .HasComment("調理目安時間");

                entity.Property(e => e.ProductCourseFlg)
                    .HasMaxLength(1)
                    .HasColumnName("product_course_flg")
                    .HasDefaultValueSql("'0'")
                    .IsFixedLength(true)
                    .HasComment("コース料理フラグ");

                entity.Property(e => e.ProductCourseOrder)
                    .HasColumnName("product_course_order")
                    .HasDefaultValueSql("'0'")
                    .HasComment("コース料理順");

                entity.Property(e => e.ProductImage)
                    .HasMaxLength(255)
                    .HasColumnName("product_image")
                    .HasComment("商品画像");

                entity.Property(e => e.ProductIntroduction)
                    .HasColumnType("text")
                    .HasColumnName("product_introduction")
                    .HasComment("商品紹介");

                entity.Property(e => e.ProductLimitedKbn)
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasColumnName("product_limited_kbn")
                    .IsFixedLength(true)
                    .HasComment("数量限定区分");

                entity.Property(e => e.ProductNameCh)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("product_name_ch")
                    .HasComment("商品名称（中国語）");

                entity.Property(e => e.ProductNameJp)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("product_name_jp")
                    .HasComment("商品名称（日本語）");

                entity.Property(e => e.ProductOffertimeFrom)
                    .HasMaxLength(5)
                    .HasColumnName("product_offertime_from")
                    .IsFixedLength(true)
                    .HasComment("提供時間帯From");

                entity.Property(e => e.ProductOffertimeTo)
                    .HasMaxLength(5)
                    .HasColumnName("product_offertime_to")
                    .IsFixedLength(true)
                    .HasComment("提供時間帯To");

                entity.Property(e => e.ProductPicupFlg)
                    .HasMaxLength(1)
                    .HasColumnName("product_picup_flg")
                    .HasDefaultValueSql("'0'")
                    .IsFixedLength(true)
                    .HasComment("お薦めフラグ");

                entity.Property(e => e.ProductPreparationFlg)
                    .HasMaxLength(1)
                    .HasColumnName("product_preparation_flg")
                    .HasDefaultValueSql("'0'")
                    .IsFixedLength(true)
                    .HasComment("準備中フラグ");

                entity.Property(e => e.ProductRemarks)
                    .HasMaxLength(50)
                    .HasColumnName("product_remarks")
                    .HasComment("商品備考");

                entity.Property(e => e.ProductSetExistenceFlg)
                    .HasMaxLength(1)
                    .HasColumnName("product_set_existence_flg")
                    .HasDefaultValueSql("'0'")
                    .IsFixedLength(true)
                    .HasComment("セット品有無フラグ");

                entity.Property(e => e.ProductStokFlg)
                    .HasMaxLength(1)
                    .HasColumnName("product_stok_flg")
                    .HasDefaultValueSql("'0'")
                    .IsFixedLength(true)
                    .HasComment("在庫連携フラグ");

                entity.Property(e => e.ProductTypeKbn)
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasColumnName("product_type_kbn")
                    .IsFixedLength(true)
                    .HasComment("商品分類区分");

                entity.Property(e => e.ProductUnitKbn)
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasColumnName("product_unit_kbn")
                    .IsFixedLength(true)
                    .HasComment("単位区分");

                entity.Property(e => e.RegistrationDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("registration_datetime")
                    .HasComment("登録日時");

                entity.Property(e => e.RegistrationUser)
                    .HasMaxLength(8)
                    .HasColumnName("registration_user")
                    .HasComment("登録者");

                entity.Property(e => e.UpdateDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("update_datetime")
                    .HasComment("更新日時");

                entity.Property(e => e.UpdateUser)
                    .HasMaxLength(8)
                    .HasColumnName("update_user")
                    .HasComment("更新者");
            });

            modelBuilder.Entity<MProductMenu>(entity =>
            {
                entity.HasKey(e => new { e.StoreNumber, e.ProductMenuNumber })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("m_product_menu");

                entity.HasComment("商品メニューマスタ")
                    .HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.StoreNumber)
                    .HasMaxLength(6)
                    .HasColumnName("store_number")
                    .IsFixedLength(true)
                    .HasComment("店舗番号");

                entity.Property(e => e.ProductMenuNumber)
                    .HasMaxLength(8)
                    .HasColumnName("product_menu_number")
                    .IsFixedLength(true)
                    .HasComment("商品メニュー管理番号");

                entity.Property(e => e.DelFlg)
                    .HasMaxLength(1)
                    .HasColumnName("del_flg")
                    .HasDefaultValueSql("'0'")
                    .IsFixedLength(true)
                    .HasComment("削除フラグ");

                entity.Property(e => e.ProductMenuApplyFlg)
                    .HasMaxLength(1)
                    .HasColumnName("product_menu_apply_flg")
                    .HasDefaultValueSql("'0'")
                    .IsFixedLength(true)
                    .HasComment("適用中フラグ");

                entity.Property(e => e.ProductMenuName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("product_menu_name")
                    .HasComment("商品メニュー名称");

                entity.Property(e => e.RegistrationDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("registration_datetime")
                    .HasComment("登録日時");

                entity.Property(e => e.RegistrationUser)
                    .HasMaxLength(8)
                    .HasColumnName("registration_user")
                    .HasComment("登録者");

                entity.Property(e => e.UpdateDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("update_datetime")
                    .HasComment("更新日時");

                entity.Property(e => e.UpdateUser)
                    .HasMaxLength(8)
                    .HasColumnName("update_user")
                    .HasComment("更新者");
            });

            modelBuilder.Entity<MProductMenuClass>(entity =>
            {
                entity.HasKey(e => new { e.StoreNumber, e.ProductMenuNumber, e.ProductMenuLineNumber })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

                entity.ToTable("m_product_menu_class");

                entity.HasComment("商品メニュー分類マスタ")
                    .HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.StoreNumber)
                    .HasMaxLength(6)
                    .HasColumnName("store_number")
                    .IsFixedLength(true)
                    .HasComment("店舗番号");

                entity.Property(e => e.ProductMenuNumber)
                    .HasMaxLength(8)
                    .HasColumnName("product_menu_number")
                    .IsFixedLength(true)
                    .HasComment("商品メニュー管理番号");

                entity.Property(e => e.ProductMenuLineNumber)
                    .HasColumnName("product_menu_line_number")
                    .HasComment("商品メニュー分類表示順");

                entity.Property(e => e.ProductMenuClassKbn)
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasColumnName("product_menu_class_kbn")
                    .IsFixedLength(true)
                    .HasComment("商品メニュー分類区分");

                entity.Property(e => e.ProductMenuNovisibleFlg)
                    .HasMaxLength(1)
                    .HasColumnName("product_menu_novisible_flg")
                    .HasDefaultValueSql("'0'")
                    .IsFixedLength(true)
                    .HasComment("商品メニュー非表示フラグ");

                entity.Property(e => e.ProductMenuTakeoutFlg)
                    .HasMaxLength(1)
                    .HasColumnName("product_menu_takeout_flg")
                    .HasDefaultValueSql("'0'")
                    .IsFixedLength(true)
                    .HasComment("持ち帰り非表示フラグ");
            });

            modelBuilder.Entity<MProductMenuItem>(entity =>
            {
                entity.HasKey(e => new { e.StoreNumber, e.ProductMenuNumber, e.ProductMenuLineNumber, e.ProductMenuOrderNumber })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0, 0 });

                entity.ToTable("m_product_menu_item");

                entity.HasComment("商品メニューアイテムマスタ")
                    .HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.StoreNumber)
                    .HasMaxLength(6)
                    .HasColumnName("store_number")
                    .IsFixedLength(true)
                    .HasComment("店舗番号");

                entity.Property(e => e.ProductMenuNumber)
                    .HasMaxLength(8)
                    .HasColumnName("product_menu_number")
                    .IsFixedLength(true)
                    .HasComment("商品メニュー管理番号");

                entity.Property(e => e.ProductMenuLineNumber)
                    .HasColumnName("product_menu_line_number")
                    .HasComment("商品メニュー分類表示順");

                entity.Property(e => e.ProductMenuOrderNumber)
                    .HasMaxLength(4)
                    .HasColumnName("product_menu_order_number")
                    .IsFixedLength(true)
                    .HasComment("商品メニュー注文番号");

                entity.Property(e => e.ProductNumber)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("product_number")
                    .IsFixedLength(true)
                    .HasComment("商品番号");
            });

            modelBuilder.Entity<MProductSet>(entity =>
            {
                entity.HasKey(e => new { e.StoreNumber, e.ProductNumber, e.ProductSetNumber })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

                entity.ToTable("m_product_set");

                entity.HasComment("商品セットマスタ")
                    .HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.StoreNumber)
                    .HasMaxLength(6)
                    .HasColumnName("store_number")
                    .IsFixedLength(true)
                    .HasComment("店舗番号");

                entity.Property(e => e.ProductNumber)
                    .HasMaxLength(8)
                    .HasColumnName("product_number")
                    .IsFixedLength(true)
                    .HasComment("商品番号");

                entity.Property(e => e.ProductSetNumber)
                    .HasMaxLength(2)
                    .HasColumnName("product_set_number")
                    .IsFixedLength(true)
                    .HasComment("商品セット番号");

                entity.Property(e => e.DelFlg)
                    .HasMaxLength(1)
                    .HasColumnName("del_flg")
                    .HasDefaultValueSql("'0'")
                    .IsFixedLength(true)
                    .HasComment("削除フラグ");

                entity.Property(e => e.ProductLimitedQuantity)
                    .HasColumnName("product_limited_quantity")
                    .HasComment("注文可能数");

                entity.Property(e => e.ProductPrice)
                    .HasColumnType("mediumint unsigned")
                    .HasColumnName("product_price")
                    .HasComment("価格（税込）");

                entity.Property(e => e.ProductSetName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("product_set_name")
                    .HasComment("商品セット名称");

                entity.Property(e => e.ProductTaxKbn)
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasColumnName("product_tax_kbn")
                    .IsFixedLength(true)
                    .HasComment("税率区分");
            });

            modelBuilder.Entity<MProductStock>(entity =>
            {
                entity.HasKey(e => new { e.StoreNumber, e.ProductNumber })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("m_product_stock");

                entity.HasComment("商品在庫管理マスタ")
                    .HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.StoreNumber)
                    .HasMaxLength(6)
                    .HasColumnName("store_number")
                    .IsFixedLength(true)
                    .HasComment("店舗番号");

                entity.Property(e => e.ProductNumber)
                    .HasMaxLength(8)
                    .HasColumnName("product_number")
                    .IsFixedLength(true)
                    .HasComment("商品番号");

                entity.Property(e => e.ProductStockQuantity)
                    .HasColumnName("product_stock_quantity")
                    .HasDefaultValueSql("'0'")
                    .HasComment("在庫数");

                entity.Property(e => e.RegistrationDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("registration_datetime")
                    .HasComment("登録日時");

                entity.Property(e => e.RegistrationUser)
                    .HasMaxLength(8)
                    .HasColumnName("registration_user")
                    .HasComment("登録者");

                entity.Property(e => e.UpdateDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("update_datetime")
                    .HasComment("更新日時");

                entity.Property(e => e.UpdateUser)
                    .HasMaxLength(8)
                    .HasColumnName("update_user")
                    .HasComment("更新者");
            });

            modelBuilder.Entity<MSeat>(entity =>
            {
                entity.HasKey(e => new { e.StoreNumber, e.SeatLevel, e.SeatNumber })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

                entity.ToTable("m_seat");

                entity.HasComment("座席マスタ")
                    .HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.StoreNumber)
                    .HasMaxLength(6)
                    .HasColumnName("store_number")
                    .IsFixedLength(true)
                    .HasComment("店舗番号");

                entity.Property(e => e.SeatLevel)
                    .HasMaxLength(1)
                    .HasColumnName("seat_level")
                    .IsFixedLength(true)
                    .HasComment("階層番号");

                entity.Property(e => e.SeatNumber)
                    .HasMaxLength(2)
                    .HasColumnName("seat_number")
                    .IsFixedLength(true)
                    .HasComment("座席番号");

                entity.Property(e => e.DelFlg)
                    .HasMaxLength(1)
                    .HasColumnName("del_flg")
                    .HasDefaultValueSql("'0'")
                    .IsFixedLength(true)
                    .HasComment("削除フラグ");

                entity.Property(e => e.RegistrationDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("registration_datetime")
                    .HasComment("登録日時");

                entity.Property(e => e.RegistrationUser)
                    .HasMaxLength(8)
                    .HasColumnName("registration_user")
                    .HasComment("登録者");

                entity.Property(e => e.SeatKbn)
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasColumnName("seat_kbn")
                    .IsFixedLength(true)
                    .HasComment("座席区分");

                entity.Property(e => e.SeatName)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("seat_name")
                    .HasComment("座席名");

                entity.Property(e => e.SeatPositionHorizontal)
                    .HasColumnName("seat_position_horizontal")
                    .HasDefaultValueSql("'0'")
                    .HasComment("座席位置横");

                entity.Property(e => e.SeatPositionVertical)
                    .HasColumnName("seat_position_vertical")
                    .HasDefaultValueSql("'0'")
                    .HasComment("座席位置縦");

                entity.Property(e => e.SeatQuantity)
                    .HasColumnName("seat_quantity")
                    .HasDefaultValueSql("'0'")
                    .HasComment("座席数");

                entity.Property(e => e.SeatSizeHorizontal)
                    .HasColumnName("seat_size_horizontal")
                    .HasDefaultValueSql("'0'")
                    .HasComment("サイズ横");

                entity.Property(e => e.SeatSizeVertical)
                    .HasColumnName("seat_size_vertical")
                    .HasDefaultValueSql("'0'")
                    .HasComment("サイズ縦");

                entity.Property(e => e.UpdateDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("update_datetime")
                    .HasComment("更新日時");

                entity.Property(e => e.UpdateUser)
                    .HasMaxLength(8)
                    .HasColumnName("update_user")
                    .HasComment("更新者");
            });

            modelBuilder.Entity<MStoreGenre>(entity =>
            {
                entity.HasKey(e => e.StoreNumber)
                    .HasName("PRIMARY");

                entity.ToTable("m_store_genre");

                entity.HasComment("店舗マスタ（ジャンル）")
                    .HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.StoreNumber)
                    .HasMaxLength(6)
                    .HasColumnName("store_number")
                    .IsFixedLength(true)
                    .HasComment("店舗番号");

                entity.Property(e => e.StoreGenreFlg)
                    .HasMaxLength(1)
                    .HasColumnName("store_genre_flg")
                    .HasDefaultValueSql("'0'")
                    .IsFixedLength(true)
                    .HasComment("店舗ジャンル有無フラグ");

                entity.Property(e => e.StoreGenreKbn)
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasColumnName("store_genre_kbn")
                    .IsFixedLength(true)
                    .HasComment("店舗ジャンル区分");
            });

            modelBuilder.Entity<MStoreInfo>(entity =>
            {
                entity.HasKey(e => e.StoreNumber)
                    .HasName("PRIMARY");

                entity.ToTable("m_store_info");

                entity.HasComment("店舗情報マスタ")
                    .HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.StoreNumber)
                    .HasMaxLength(6)
                    .HasColumnName("store_number")
                    .IsFixedLength(true)
                    .HasComment("店舗番号");

                entity.Property(e => e.RegistrationDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("registration_datetime")
                    .HasComment("登録日時");

                entity.Property(e => e.RegistrationUser)
                    .HasMaxLength(8)
                    .HasColumnName("registration_user")
                    .HasComment("登録者");

                entity.Property(e => e.StoreAddress)
                    .HasMaxLength(255)
                    .HasColumnName("store_address")
                    .HasComment("住所");

                entity.Property(e => e.StoreAddress2)
                    .HasMaxLength(255)
                    .HasColumnName("store_address2");

                entity.Property(e => e.StoreAllergiesDisplayFlg)
                    .HasMaxLength(1)
                    .HasColumnName("store_allergies_display_flg")
                    .HasDefaultValueSql("'0'")
                    .IsFixedLength(true)
                    .HasComment("アレルギー表示有無フラグ");

                entity.Property(e => e.StoreHolidaysFrom)
                    .HasMaxLength(5)
                    .HasColumnName("store_holidays_from")
                    .IsFixedLength(true)
                    .HasComment("営業時間日祝From");

                entity.Property(e => e.StoreHolidaysTo)
                    .HasMaxLength(5)
                    .HasColumnName("store_holidays_to")
                    .IsFixedLength(true)
                    .HasComment("営業時間日祝To");

                entity.Property(e => e.StoreImage)
                    .HasMaxLength(255)
                    .HasColumnName("store_image")
                    .HasComment("店舗写真");

                entity.Property(e => e.StoreIntroduction)
                    .HasColumnType("text")
                    .HasColumnName("store_introduction")
                    .HasComment("紹介");

                entity.Property(e => e.StoreLogoImage)
                    .HasMaxLength(255)
                    .HasColumnName("store_logo_image")
                    .HasComment("店舗ロゴ");

                entity.Property(e => e.StoreName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("store_name")
                    .HasComment("店舗名");

                entity.Property(e => e.StoreSaturdayFrom)
                    .HasMaxLength(5)
                    .HasColumnName("store_saturday_from")
                    .IsFixedLength(true)
                    .HasComment("営業時間土曜日From");

                entity.Property(e => e.StoreSaturdayTo)
                    .HasMaxLength(5)
                    .HasColumnName("store_saturday_to")
                    .IsFixedLength(true)
                    .HasComment("営業時間土曜日To");

                entity.Property(e => e.StoreSeatQuantity)
                    .HasColumnName("store_seat_quantity")
                    .HasComment("座席数");

                entity.Property(e => e.StoreStaffWord)
                    .HasColumnType("text")
                    .HasColumnName("store_staff_word")
                    .HasComment("一言");

                entity.Property(e => e.StoreTel)
                    .HasMaxLength(15)
                    .HasColumnName("store_tel")
                    .HasComment("電話番号");

                entity.Property(e => e.StoreUrl)
                    .HasMaxLength(255)
                    .HasColumnName("store_url")
                    .HasComment("URL");

                entity.Property(e => e.StoreWeekdaysFrom)
                    .HasMaxLength(5)
                    .HasColumnName("store_weekdays_from")
                    .IsFixedLength(true)
                    .HasComment("営業時間平日From");

                entity.Property(e => e.StoreWeekdaysTo)
                    .HasMaxLength(5)
                    .HasColumnName("store_weekdays_to")
                    .IsFixedLength(true)
                    .HasComment("営業時間平日To");

                entity.Property(e => e.UpdateDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("update_datetime")
                    .HasComment("更新日時");

                entity.Property(e => e.UpdateUser)
                    .HasMaxLength(8)
                    .HasColumnName("update_user")
                    .HasComment("更新者");
            });

            modelBuilder.Entity<MSystemMenu>(entity =>
            {
                entity.HasKey(e => new { e.StoreNumber, e.SystemMenuUserKbn, e.SystemMenuYPosition, e.SystemMenuXPosition })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0, 0 });

                entity.ToTable("m_system_menu");

                entity.HasComment("システムメニューマスタ")
                    .HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.StoreNumber)
                    .HasMaxLength(6)
                    .HasColumnName("store_number")
                    .IsFixedLength(true)
                    .HasComment("店舗番号");

                entity.Property(e => e.SystemMenuUserKbn)
                    .HasMaxLength(3)
                    .HasColumnName("system_menu_user_kbn")
                    .IsFixedLength(true)
                    .HasComment("利用者区分");

                entity.Property(e => e.SystemMenuYPosition)
                    .HasColumnName("system_menu_y_position")
                    .HasComment("ボタン縦位置");

                entity.Property(e => e.SystemMenuXPosition)
                    .HasColumnName("system_menu_x_position")
                    .HasComment("ボタン横位置");

                entity.Property(e => e.RegistrationDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("registration_datetime")
                    .HasComment("登録日時");

                entity.Property(e => e.RegistrationUser)
                    .HasMaxLength(8)
                    .HasColumnName("registration_user")
                    .HasComment("登録者");

                entity.Property(e => e.SystemMenuButtonColor)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("system_menu_button_color")
                    .HasComment("ボタン色");

                entity.Property(e => e.SystemMenuButtonIcon)
                    .HasMaxLength(5)
                    .HasColumnName("system_menu_button_icon")
                    .HasComment("ボタンアイコン");

                entity.Property(e => e.SystemMenuFontColor)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("system_menu_font_color")
                    .HasComment("フォント色");

                entity.Property(e => e.SystemMenuId)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("system_menu_id")
                    .HasComment("画面ID");

                entity.Property(e => e.SystemMenuLink)
                    .HasMaxLength(255)
                    .HasColumnName("system_menu_link")
                    .HasComment("ボタンリンク");

                entity.Property(e => e.SystemMenuName)
                    .HasMaxLength(10)
                    .HasColumnName("system_menu_name")
                    .HasComment("ボタン名");

                entity.Property(e => e.SystemMenuUnusedFlg)
                    .HasMaxLength(1)
                    .HasColumnName("system_menu_unused_flg")
                    .HasDefaultValueSql("'0'")
                    .IsFixedLength(true)
                    .HasComment("未使用フラグ");

                entity.Property(e => e.UpdateDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("update_datetime")
                    .HasComment("更新日時");

                entity.Property(e => e.UpdateUser)
                    .HasMaxLength(8)
                    .HasColumnName("update_user")
                    .HasComment("更新者");
            });

            modelBuilder.Entity<MTax>(entity =>
            {
                entity.HasKey(e => new { e.StoreNumber, e.TaxKbn, e.TaxDateStart })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

                entity.ToTable("m_tax");

                entity.HasComment("税区分マスタ")
                    .HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.StoreNumber)
                    .HasMaxLength(6)
                    .HasColumnName("store_number")
                    .IsFixedLength(true)
                    .HasComment("店舗番号");

                entity.Property(e => e.TaxKbn)
                    .HasMaxLength(3)
                    .HasColumnName("tax_kbn")
                    .IsFixedLength(true)
                    .HasComment("税区分");

                entity.Property(e => e.TaxDateStart)
                    .HasMaxLength(8)
                    .HasColumnName("tax_date_start")
                    .IsFixedLength(true)
                    .HasComment("適用開始日");

                entity.Property(e => e.RegistrationDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("registration_datetime")
                    .HasComment("登録日時");

                entity.Property(e => e.RegistrationUser)
                    .HasMaxLength(8)
                    .HasColumnName("registration_user")
                    .HasComment("登録者");

                entity.Property(e => e.TaxDateEnd)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("tax_date_end")
                    .IsFixedLength(true)
                    .HasComment("適用終了日");

                entity.Property(e => e.TaxPercent)
                    .HasColumnName("tax_percent")
                    .HasComment("税率");

                entity.Property(e => e.UpdateDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("update_datetime")
                    .HasComment("更新日時");

                entity.Property(e => e.UpdateUser)
                    .HasMaxLength(8)
                    .HasColumnName("update_user")
                    .HasComment("更新者");
            });

            modelBuilder.Entity<TEquipmentConsumption>(entity =>
            {
                entity.HasKey(e => new { e.StoreNumber, e.EquipmentNumber, e.EquipmentConsumptionDate, e.EquipmentConsumptionTimes, e.EquipmentConsumptionKbn })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0, 0, 0 });

                entity.ToTable("t_equipment_consumption");

                entity.HasComment("備品消費テーブル")
                    .HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.StoreNumber)
                    .HasMaxLength(6)
                    .HasColumnName("store_number")
                    .IsFixedLength(true)
                    .HasComment("店舗番号");

                entity.Property(e => e.EquipmentNumber)
                    .HasMaxLength(8)
                    .HasColumnName("equipment_number")
                    .IsFixedLength(true)
                    .HasComment("備品番号");

                entity.Property(e => e.EquipmentConsumptionDate)
                    .HasMaxLength(8)
                    .HasColumnName("equipment_consumption_date")
                    .IsFixedLength(true)
                    .HasComment("備品消費日");

                entity.Property(e => e.EquipmentConsumptionTimes)
                    .HasColumnName("equipment_consumption_times")
                    .HasComment("備品消費処理回数");

                entity.Property(e => e.EquipmentConsumptionKbn)
                    .HasMaxLength(3)
                    .HasColumnName("equipment_consumption_kbn")
                    .IsFixedLength(true)
                    .HasComment("備品消費区分");

                entity.Property(e => e.EquipmentConsumptionQuantity)
                    .HasColumnName("equipment_consumption_quantity")
                    .HasDefaultValueSql("'0'")
                    .HasComment("消費数");

                entity.Property(e => e.RegistrationDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("registration_datetime")
                    .HasComment("登録日時");

                entity.Property(e => e.RegistrationUser)
                    .HasMaxLength(8)
                    .HasColumnName("registration_user")
                    .HasComment("登録者");

                entity.Property(e => e.UpdateDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("update_datetime")
                    .HasComment("更新日時");

                entity.Property(e => e.UpdateUser)
                    .HasMaxLength(8)
                    .HasColumnName("update_user")
                    .HasComment("更新者");
            });

            modelBuilder.Entity<TEquipmentInventory>(entity =>
            {
                entity.HasKey(e => new { e.StoreNumber, e.EquipmentInventoryDate, e.EquipmentNumber })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

                entity.ToTable("t_equipment_inventory");

                entity.HasComment("備品棚卸テーブル")
                    .HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.StoreNumber)
                    .HasMaxLength(6)
                    .HasColumnName("store_number")
                    .IsFixedLength(true)
                    .HasComment("店舗番号");

                entity.Property(e => e.EquipmentInventoryDate)
                    .HasMaxLength(8)
                    .HasColumnName("equipment_inventory_date")
                    .IsFixedLength(true)
                    .HasComment("備品棚卸日");

                entity.Property(e => e.EquipmentNumber)
                    .HasMaxLength(8)
                    .HasColumnName("equipment_number")
                    .IsFixedLength(true)
                    .HasComment("備品番号");

                entity.Property(e => e.EquipmentInventoryQuantity)
                    .HasColumnName("equipment_inventory_quantity")
                    .HasDefaultValueSql("'0'")
                    .HasComment("棚卸数");

                entity.Property(e => e.RegistrationDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("registration_datetime")
                    .HasComment("登録日時");

                entity.Property(e => e.RegistrationUser)
                    .HasMaxLength(8)
                    .HasColumnName("registration_user")
                    .HasComment("登録者");

                entity.Property(e => e.UpdateDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("update_datetime")
                    .HasComment("更新日時");

                entity.Property(e => e.UpdateUser)
                    .HasMaxLength(8)
                    .HasColumnName("update_user")
                    .HasComment("更新者");
            });

            modelBuilder.Entity<TEquipmentStoringIssue>(entity =>
            {
                entity.HasKey(e => new { e.StoreNumber, e.EquipmentNumber, e.EquipmentStoringIssueDate, e.EquipmentStoringIssueTimes, e.EquipmentStoringIssueKbn })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0, 0, 0 });

                entity.ToTable("t_equipment_storing_issue");

                entity.HasComment("備品入出庫テーブル")
                    .HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.StoreNumber)
                    .HasMaxLength(6)
                    .HasColumnName("store_number")
                    .IsFixedLength(true)
                    .HasComment("店舗番号");

                entity.Property(e => e.EquipmentNumber)
                    .HasMaxLength(8)
                    .HasColumnName("equipment_number")
                    .IsFixedLength(true)
                    .HasComment("備品番号");

                entity.Property(e => e.EquipmentStoringIssueDate)
                    .HasMaxLength(8)
                    .HasColumnName("equipment_storing_issue_date")
                    .IsFixedLength(true)
                    .HasComment("備品入出庫日");

                entity.Property(e => e.EquipmentStoringIssueTimes)
                    .HasColumnName("equipment_storing_issue_times")
                    .HasComment("備品入出庫処理回数");

                entity.Property(e => e.EquipmentStoringIssueKbn)
                    .HasMaxLength(3)
                    .HasColumnName("equipment_storing_issue_kbn")
                    .IsFixedLength(true)
                    .HasComment("備品入出庫区分");

                entity.Property(e => e.EquipmentStoringIssueQuantity)
                    .HasColumnName("equipment_storing_issue_quantity")
                    .HasDefaultValueSql("'0'")
                    .HasComment("入出庫数");

                entity.Property(e => e.EquipmentStoringIssueRemarks)
                    .HasMaxLength(255)
                    .HasColumnName("equipment_storing_issue_remarks")
                    .HasDefaultValueSql("'0'")
                    .HasComment("入出庫備考");

                entity.Property(e => e.RegistrationDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("registration_datetime")
                    .HasComment("登録日時");

                entity.Property(e => e.RegistrationUser)
                    .HasMaxLength(8)
                    .HasColumnName("registration_user")
                    .HasComment("登録者");

                entity.Property(e => e.UpdateDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("update_datetime")
                    .HasComment("更新日時");

                entity.Property(e => e.UpdateUser)
                    .HasMaxLength(8)
                    .HasColumnName("update_user")
                    .HasComment("更新者");
            });

            modelBuilder.Entity<TMaterialConsumption>(entity =>
            {
                entity.HasKey(e => new { e.StoreNumber, e.MaterialNumber, e.MaterialConsumptionDate, e.MaterialConsumptionTimes, e.MaterialConsumptionKbn })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0, 0, 0 });

                entity.ToTable("t_material_consumption");

                entity.HasComment("原材料消費テーブル")
                    .HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.StoreNumber)
                    .HasMaxLength(6)
                    .HasColumnName("store_number")
                    .IsFixedLength(true)
                    .HasComment("店舗番号");

                entity.Property(e => e.MaterialNumber)
                    .HasMaxLength(8)
                    .HasColumnName("material_number")
                    .IsFixedLength(true)
                    .HasComment("原材料番号");

                entity.Property(e => e.MaterialConsumptionDate)
                    .HasMaxLength(8)
                    .HasColumnName("material_consumption_date")
                    .IsFixedLength(true)
                    .HasComment("原材料消費日");

                entity.Property(e => e.MaterialConsumptionTimes)
                    .HasColumnName("material_consumption_times")
                    .HasComment("原材料消費処理回数");

                entity.Property(e => e.MaterialConsumptionKbn)
                    .HasMaxLength(3)
                    .HasColumnName("material_consumption_kbn")
                    .IsFixedLength(true)
                    .HasComment("原材料消費区分");

                entity.Property(e => e.MaterialConsumptionQuantity)
                    .HasColumnName("material_consumption_quantity")
                    .HasDefaultValueSql("'0'")
                    .HasComment("消費数");

                entity.Property(e => e.RegistrationDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("registration_datetime")
                    .HasComment("登録日時");

                entity.Property(e => e.RegistrationUser)
                    .HasMaxLength(8)
                    .HasColumnName("registration_user")
                    .HasComment("登録者");

                entity.Property(e => e.UpdateDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("update_datetime")
                    .HasComment("更新日時");

                entity.Property(e => e.UpdateUser)
                    .HasMaxLength(8)
                    .HasColumnName("update_user")
                    .HasComment("更新者");
            });

            modelBuilder.Entity<TMaterialInventory>(entity =>
            {
                entity.HasKey(e => new { e.StoreNumber, e.MaterialInventoryDate, e.MaterialNumber })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

                entity.ToTable("t_material_inventory");

                entity.HasComment("原材料棚卸テーブル")
                    .HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.StoreNumber)
                    .HasMaxLength(6)
                    .HasColumnName("store_number")
                    .IsFixedLength(true)
                    .HasComment("店舗番号");

                entity.Property(e => e.MaterialInventoryDate)
                    .HasMaxLength(8)
                    .HasColumnName("material_inventory_date")
                    .IsFixedLength(true)
                    .HasComment("原材料棚卸日");

                entity.Property(e => e.MaterialNumber)
                    .HasMaxLength(8)
                    .HasColumnName("material_number")
                    .IsFixedLength(true)
                    .HasComment("原材料番号");

                entity.Property(e => e.MaterialInventoryQuantity)
                    .HasColumnName("material_inventory_quantity")
                    .HasDefaultValueSql("'0'")
                    .HasComment("棚卸数");

                entity.Property(e => e.RegistrationDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("registration_datetime")
                    .HasComment("登録日時");

                entity.Property(e => e.RegistrationUser)
                    .HasMaxLength(8)
                    .HasColumnName("registration_user")
                    .HasComment("登録者");

                entity.Property(e => e.UpdateDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("update_datetime")
                    .HasComment("更新日時");

                entity.Property(e => e.UpdateUser)
                    .HasMaxLength(8)
                    .HasColumnName("update_user")
                    .HasComment("更新者");
            });

            modelBuilder.Entity<TMaterialStoringIssue>(entity =>
            {
                entity.HasKey(e => new { e.StoreNumber, e.MaterialNumber, e.MaterialStoringIssueDate, e.MaterialStoringIssueTimes, e.MaterialStoringIssueKbn })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0, 0, 0 });

                entity.ToTable("t_material_storing_issue");

                entity.HasComment("原材料入出庫テーブル")
                    .HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.StoreNumber)
                    .HasMaxLength(6)
                    .HasColumnName("store_number")
                    .IsFixedLength(true)
                    .HasComment("店舗番号");

                entity.Property(e => e.MaterialNumber)
                    .HasMaxLength(8)
                    .HasColumnName("material_number")
                    .IsFixedLength(true)
                    .HasComment("原材料番号");

                entity.Property(e => e.MaterialStoringIssueDate)
                    .HasMaxLength(8)
                    .HasColumnName("material_storing_issue_date")
                    .IsFixedLength(true)
                    .HasComment("原材料入出庫日");

                entity.Property(e => e.MaterialStoringIssueTimes)
                    .HasColumnName("material_storing_issue_times")
                    .HasComment("原材料入出庫処理回数");

                entity.Property(e => e.MaterialStoringIssueKbn)
                    .HasMaxLength(3)
                    .HasColumnName("material_storing_issue_kbn")
                    .IsFixedLength(true)
                    .HasComment("原材料入出庫区分");

                entity.Property(e => e.MaterialStoringIssueQuantity)
                    .HasColumnName("material_storing_issue_quantity")
                    .HasDefaultValueSql("'0'")
                    .HasComment("入出庫数");

                entity.Property(e => e.MaterialStoringIssueRemarks)
                    .HasMaxLength(255)
                    .HasColumnName("material_storing_issue_remarks")
                    .HasDefaultValueSql("'0'")
                    .HasComment("入出庫備考");

                entity.Property(e => e.RegistrationDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("registration_datetime")
                    .HasComment("登録日時");

                entity.Property(e => e.RegistrationUser)
                    .HasMaxLength(8)
                    .HasColumnName("registration_user")
                    .HasComment("登録者");

                entity.Property(e => e.UpdateDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("update_datetime")
                    .HasComment("更新日時");

                entity.Property(e => e.UpdateUser)
                    .HasMaxLength(8)
                    .HasColumnName("update_user")
                    .HasComment("更新者");
            });

            modelBuilder.Entity<TMoneyInout>(entity =>
            {
                entity.HasKey(e => new { e.StoreNumber, e.MoneyInoutNumber })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("t_money_inout");

                entity.HasComment("入出金テーブル")
                    .HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.StoreNumber)
                    .HasMaxLength(6)
                    .HasColumnName("store_number")
                    .IsFixedLength(true)
                    .HasComment("店舗番号");

                entity.Property(e => e.MoneyInoutNumber)
                    .HasMaxLength(11)
                    .HasColumnName("money_inout_number")
                    .IsFixedLength(true)
                    .HasComment("入出金番号");

                entity.Property(e => e.ClosingFlg)
                    .HasMaxLength(1)
                    .HasColumnName("closing_flg")
                    .HasDefaultValueSql("'0'")
                    .IsFixedLength(true)
                    .HasComment("締フラグ");

                entity.Property(e => e.MoneyInoutDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("money_inout_datetime")
                    .HasComment("入出金日時");

                entity.Property(e => e.MoneyInoutKbn)
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasColumnName("money_inout_kbn")
                    .IsFixedLength(true)
                    .HasComment("入出金区分");

                entity.Property(e => e.MoneyInoutOriginNumber)
                    .HasMaxLength(11)
                    .HasColumnName("money_inout_origin_number")
                    .IsFixedLength(true)
                    .HasComment("元入出金番号");

                entity.Property(e => e.MoneyInoutPrice)
                    .HasColumnType("mediumint unsigned")
                    .HasColumnName("money_inout_price")
                    .HasDefaultValueSql("'0'")
                    .HasComment("金額");

                entity.Property(e => e.MoneyInoutRbFlg)
                    .HasMaxLength(1)
                    .HasColumnName("money_inout_rb_flg")
                    .HasDefaultValueSql("'0'")
                    .IsFixedLength(true)
                    .HasComment("赤黒フラグ");

                entity.Property(e => e.MoneyInoutReasonKbn)
                    .HasMaxLength(3)
                    .HasColumnName("money_inout_reason_kbn")
                    .IsFixedLength(true)
                    .HasComment("入出金理由区分");

                entity.Property(e => e.MoneyInoutRemarks)
                    .HasMaxLength(255)
                    .HasColumnName("money_inout_remarks")
                    .HasComment("備考");

                entity.Property(e => e.RegistrationDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("registration_datetime")
                    .HasComment("登録日時");

                entity.Property(e => e.RegistrationUser)
                    .HasMaxLength(8)
                    .HasColumnName("registration_user")
                    .HasComment("登録者");

                entity.Property(e => e.UpdateDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("update_datetime")
                    .HasComment("更新日時");

                entity.Property(e => e.UpdateUser)
                    .HasMaxLength(8)
                    .HasColumnName("update_user")
                    .HasComment("更新者");
            });

            modelBuilder.Entity<TOrder>(entity =>
            {
                entity.HasKey(e => new { e.StoreNumber, e.ReceptionNumber, e.ReceptionBranchNumber, e.OrderVoucherNumber, e.OrderVoucherDetails })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0, 0, 0 });

                entity.ToTable("t_order");

                entity.HasComment("注文テーブル")
                    .HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.StoreNumber)
                    .HasMaxLength(6)
                    .HasColumnName("store_number")
                    .IsFixedLength(true)
                    .HasComment("店舗番号");

                entity.Property(e => e.ReceptionNumber)
                    .HasMaxLength(11)
                    .HasColumnName("reception_number")
                    .IsFixedLength(true)
                    .HasComment("受付番号");

                entity.Property(e => e.ReceptionBranchNumber)
                    .HasColumnName("reception_branch_number")
                    .HasComment("受付枝番");

                entity.Property(e => e.OrderVoucherNumber)
                    .HasMaxLength(8)
                    .HasColumnName("order_voucher_number")
                    .IsFixedLength(true)
                    .HasComment("注文伝票番号");

                entity.Property(e => e.OrderVoucherDetails)
                    .HasMaxLength(3)
                    .HasColumnName("order_voucher_details")
                    .IsFixedLength(true)
                    .HasComment("注文伝票明細番号");

                entity.Property(e => e.OrderAfterPrice)
                    .HasColumnType("mediumint unsigned")
                    .HasColumnName("order_after_price")
                    .HasDefaultValueSql("'0'")
                    .HasComment("値引き後金額");

                entity.Property(e => e.OrderCancelFlg)
                    .HasMaxLength(1)
                    .HasColumnName("order_cancel_flg")
                    .HasDefaultValueSql("'0'")
                    .IsFixedLength(true)
                    .HasComment("取消フラグ");

                entity.Property(e => e.OrderCookingFlg)
                    .HasMaxLength(1)
                    .HasColumnName("order_cooking_flg")
                    .HasDefaultValueSql("'0'")
                    .IsFixedLength(true)
                    .HasComment("調理指示フラグ");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("datetime")
                    .HasColumnName("order_date")
                    .HasComment("注文日時");

                entity.Property(e => e.OrderDiscountPercent)
                    .HasColumnName("order_discount_percent")
                    .HasDefaultValueSql("'0'")
                    .HasComment("割引");

                entity.Property(e => e.OrderDiscountYen)
                    .HasColumnType("mediumint unsigned")
                    .HasColumnName("order_discount_yen")
                    .HasDefaultValueSql("'0'")
                    .HasComment("値引き後価格");

                entity.Property(e => e.OrderPrice)
                    .HasColumnType("mediumint unsigned")
                    .HasColumnName("order_price")
                    .HasDefaultValueSql("'0'")
                    .HasComment("金額");

                entity.Property(e => e.OrderQuantity)
                    .HasColumnName("order_quantity")
                    .HasDefaultValueSql("'0'")
                    .HasComment("個数");

                entity.Property(e => e.OrderRemarks)
                    .HasMaxLength(255)
                    .HasColumnName("order_remarks")
                    .HasComment("備考");

                entity.Property(e => e.OrderServeFlg)
                    .HasMaxLength(1)
                    .HasColumnName("order_serve_flg")
                    .HasDefaultValueSql("'0'")
                    .IsFixedLength(true)
                    .HasComment("配膳完了フラグ");

                entity.Property(e => e.OrderTakeoutFlg)
                    .HasMaxLength(1)
                    .HasColumnName("order_takeout_flg")
                    .HasDefaultValueSql("'0'")
                    .IsFixedLength(true)
                    .HasComment("持帰りフラグ");

                entity.Property(e => e.PaymentNumber)
                    .HasMaxLength(11)
                    .HasColumnName("payment_number")
                    .IsFixedLength(true)
                    .HasComment("会計番号");

                entity.Property(e => e.ProductNameCh)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("product_name_ch")
                    .HasComment("商品名称（中国語）");

                entity.Property(e => e.ProductNumber)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("product_number")
                    .IsFixedLength(true)
                    .HasComment("商品番号");

                entity.Property(e => e.ProductPrice)
                    .HasColumnType("mediumint unsigned")
                    .HasColumnName("product_price")
                    .HasDefaultValueSql("'0'")
                    .HasComment("価格（税込）");

                entity.Property(e => e.ProductSetName)
                    .HasMaxLength(20)
                    .HasColumnName("product_set_name")
                    .HasComment("商品セット名称");

                entity.Property(e => e.ProductSetNumber)
                    .IsRequired()
                    .HasMaxLength(2)
                    .HasColumnName("product_set_number")
                    .IsFixedLength(true)
                    .HasComment("商品セット番号");

                entity.Property(e => e.ProductTypeKbn)
                    .HasMaxLength(3)
                    .HasColumnName("product_type_kbn")
                    .IsFixedLength(true)
                    .HasComment("商品分類区分");

                entity.Property(e => e.RegistrationDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("registration_datetime")
                    .HasComment("登録日時");

                entity.Property(e => e.RegistrationUser)
                    .HasMaxLength(8)
                    .HasColumnName("registration_user")
                    .HasComment("登録者");

                entity.Property(e => e.UpdateDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("update_datetime")
                    .HasComment("更新日時");

                entity.Property(e => e.UpdateUser)
                    .HasMaxLength(8)
                    .HasColumnName("update_user")
                    .HasComment("更新者");
            });

            modelBuilder.Entity<TPayment>(entity =>
            {
                entity.HasKey(e => new { e.StoreNumber, e.PaymentNumber })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("t_payment");

                entity.HasComment("会計テーブル")
                    .HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.StoreNumber)
                    .HasMaxLength(6)
                    .HasColumnName("store_number")
                    .IsFixedLength(true)
                    .HasComment("店舗番号");

                entity.Property(e => e.PaymentNumber)
                    .HasMaxLength(11)
                    .HasColumnName("payment_number")
                    .IsFixedLength(true)
                    .HasComment("会計番号");

                entity.Property(e => e.ClosingFlg)
                    .HasMaxLength(1)
                    .HasColumnName("closing_flg")
                    .HasDefaultValueSql("'0'")
                    .IsFixedLength(true)
                    .HasComment("締フラグ");

                entity.Property(e => e.PaymentClassKbn)
                    .HasMaxLength(3)
                    .HasColumnName("payment_class_kbn")
                    .HasDefaultValueSql("'0'")
                    .IsFixedLength(true)
                    .HasComment("会計種類分類区分");

                entity.Property(e => e.PaymentConfirmPrice)
                    .HasColumnName("payment_confirm_price")
                    .HasDefaultValueSql("'0'")
                    .HasComment("確定金額");

                entity.Property(e => e.PaymentDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("payment_datetime")
                    .HasComment("会計日時");

                entity.Property(e => e.PaymentKbn)
                    .HasMaxLength(3)
                    .HasColumnName("payment_kbn")
                    .HasDefaultValueSql("'0'")
                    .IsFixedLength(true)
                    .HasComment("会計種類区分");

                entity.Property(e => e.PaymentOriginNumber)
                    .HasMaxLength(11)
                    .HasColumnName("payment_origin_number")
                    .IsFixedLength(true)
                    .HasComment("元会計番号");

                entity.Property(e => e.PaymentPrice)
                    .HasColumnType("mediumint unsigned")
                    .HasColumnName("payment_price")
                    .HasDefaultValueSql("'0'")
                    .HasComment("会計金額");

                entity.Property(e => e.PaymentRbFlg)
                    .HasMaxLength(1)
                    .HasColumnName("payment_rb_flg")
                    .HasDefaultValueSql("'0'")
                    .IsFixedLength(true)
                    .HasComment("赤黒フラグ");

                entity.Property(e => e.PaymentRemarks)
                    .HasMaxLength(255)
                    .HasColumnName("payment_remarks")
                    .HasComment("備考");

                entity.Property(e => e.RegistrationDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("registration_datetime")
                    .HasComment("登録日時");

                entity.Property(e => e.RegistrationUser)
                    .HasMaxLength(8)
                    .HasColumnName("registration_user")
                    .HasComment("登録者");

                entity.Property(e => e.UpdateDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("update_datetime")
                    .HasComment("更新日時");

                entity.Property(e => e.UpdateUser)
                    .HasMaxLength(8)
                    .HasColumnName("update_user")
                    .HasComment("更新者");
            });

            modelBuilder.Entity<TPaymentDiscount>(entity =>
            {
                entity.HasKey(e => new { e.StoreNumber, e.PaymentNumber, e.PaymentBranchNumber })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

                entity.ToTable("t_payment_discount");

                entity.HasComment("会計割引割増テーブル")
                    .HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.StoreNumber)
                    .HasMaxLength(6)
                    .HasColumnName("store_number")
                    .IsFixedLength(true)
                    .HasComment("店舗番号");

                entity.Property(e => e.PaymentNumber)
                    .HasMaxLength(11)
                    .HasColumnName("payment_number")
                    .IsFixedLength(true)
                    .HasComment("会計番号");

                entity.Property(e => e.PaymentBranchNumber)
                    .HasColumnName("payment_branch_number")
                    .HasComment("枝番");

                entity.Property(e => e.DiscountPercent)
                    .HasColumnName("discount_percent")
                    .HasDefaultValueSql("'0'")
                    .HasComment("割引");

                entity.Property(e => e.DiscountPremiumKbn)
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasColumnName("discount_premium_kbn")
                    .IsFixedLength(true)
                    .HasComment("割引割増区分");

                entity.Property(e => e.DiscountYen)
                    .HasColumnType("mediumint unsigned")
                    .HasColumnName("discount_yen")
                    .HasDefaultValueSql("'0'")
                    .HasComment("値引き");

                entity.Property(e => e.PremiumPercent)
                    .HasColumnName("premium_percent")
                    .HasDefaultValueSql("'0'")
                    .HasComment("割増");

                entity.Property(e => e.PremiumYen)
                    .HasColumnType("mediumint unsigned")
                    .HasColumnName("premium_yen")
                    .HasDefaultValueSql("'0'")
                    .HasComment("値上げ");

                entity.Property(e => e.RegistrationDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("registration_datetime")
                    .HasComment("登録日時");

                entity.Property(e => e.RegistrationUser)
                    .HasMaxLength(8)
                    .HasColumnName("registration_user")
                    .HasComment("登録者");

                entity.Property(e => e.UpdateDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("update_datetime")
                    .HasComment("更新日時");

                entity.Property(e => e.UpdateUser)
                    .HasMaxLength(8)
                    .HasColumnName("update_user")
                    .HasComment("更新者");
            });

            modelBuilder.Entity<TProductConsumption>(entity =>
            {
                entity.HasKey(e => new { e.StoreNumber, e.ProductNumber, e.ProductConsumptionDate, e.ProductConsumptionTimes, e.ProductConsumptionKbn })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0, 0, 0 });

                entity.ToTable("t_product_consumption");

                entity.HasComment("商品消費テーブル")
                    .HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.StoreNumber)
                    .HasMaxLength(6)
                    .HasColumnName("store_number")
                    .IsFixedLength(true)
                    .HasComment("店舗番号");

                entity.Property(e => e.ProductNumber)
                    .HasMaxLength(8)
                    .HasColumnName("product_number")
                    .IsFixedLength(true)
                    .HasComment("商品番号");

                entity.Property(e => e.ProductConsumptionDate)
                    .HasMaxLength(8)
                    .HasColumnName("product_consumption_date")
                    .IsFixedLength(true)
                    .HasComment("商品消費日");

                entity.Property(e => e.ProductConsumptionTimes)
                    .HasColumnName("product_consumption_times")
                    .HasComment("商品消費処理回数");

                entity.Property(e => e.ProductConsumptionKbn)
                    .HasMaxLength(3)
                    .HasColumnName("product_consumption_kbn")
                    .IsFixedLength(true)
                    .HasComment("商品消費区分");

                entity.Property(e => e.ProductConsumptionQuantity)
                    .HasColumnName("product_consumption_quantity")
                    .HasDefaultValueSql("'0'")
                    .HasComment("消費数");

                entity.Property(e => e.RegistrationDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("registration_datetime")
                    .HasComment("登録日時");

                entity.Property(e => e.RegistrationUser)
                    .HasMaxLength(8)
                    .HasColumnName("registration_user")
                    .HasComment("登録者");

                entity.Property(e => e.UpdateDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("update_datetime")
                    .HasComment("更新日時");

                entity.Property(e => e.UpdateUser)
                    .HasMaxLength(8)
                    .HasColumnName("update_user")
                    .HasComment("更新者");
            });

            modelBuilder.Entity<TProductInventory>(entity =>
            {
                entity.HasKey(e => new { e.StoreNumber, e.ProductInventoryDate, e.ProductNumber })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

                entity.ToTable("t_product_inventory");

                entity.HasComment("商品棚卸テーブル")
                    .HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.StoreNumber)
                    .HasMaxLength(6)
                    .HasColumnName("store_number")
                    .IsFixedLength(true)
                    .HasComment("店舗番号");

                entity.Property(e => e.ProductInventoryDate)
                    .HasMaxLength(8)
                    .HasColumnName("product_inventory_date")
                    .IsFixedLength(true)
                    .HasComment("商品棚卸日");

                entity.Property(e => e.ProductNumber)
                    .HasMaxLength(8)
                    .HasColumnName("product_number")
                    .IsFixedLength(true)
                    .HasComment("商品番号");

                entity.Property(e => e.ProductInventoryQuantity)
                    .HasColumnName("product_inventory_quantity")
                    .HasDefaultValueSql("'0'")
                    .HasComment("棚卸数");

                entity.Property(e => e.RegistrationDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("registration_datetime")
                    .HasComment("登録日時");

                entity.Property(e => e.RegistrationUser)
                    .HasMaxLength(8)
                    .HasColumnName("registration_user")
                    .HasComment("登録者");

                entity.Property(e => e.UpdateDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("update_datetime")
                    .HasComment("更新日時");

                entity.Property(e => e.UpdateUser)
                    .HasMaxLength(8)
                    .HasColumnName("update_user")
                    .HasComment("更新者");
            });

            modelBuilder.Entity<TProductStoringIssue>(entity =>
            {
                entity.HasKey(e => new { e.StoreNumber, e.ProductNumber, e.ProductStoringIssueDate, e.ProductStoringIssueTimes, e.ProductStoringIssueKbn })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0, 0, 0 });

                entity.ToTable("t_product_storing_issue");

                entity.HasComment("商品入出庫テーブル")
                    .HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.StoreNumber)
                    .HasMaxLength(6)
                    .HasColumnName("store_number")
                    .IsFixedLength(true)
                    .HasComment("店舗番号");

                entity.Property(e => e.ProductNumber)
                    .HasMaxLength(8)
                    .HasColumnName("product_number")
                    .IsFixedLength(true)
                    .HasComment("商品番号");

                entity.Property(e => e.ProductStoringIssueDate)
                    .HasMaxLength(8)
                    .HasColumnName("product_storing_issue_date")
                    .IsFixedLength(true)
                    .HasComment("商品入出庫日");

                entity.Property(e => e.ProductStoringIssueTimes)
                    .HasColumnName("product_storing_issue_times")
                    .HasComment("商品入出庫処理回数");

                entity.Property(e => e.ProductStoringIssueKbn)
                    .HasMaxLength(3)
                    .HasColumnName("product_storing_issue_kbn")
                    .IsFixedLength(true)
                    .HasComment("商品入出庫区分");

                entity.Property(e => e.ProductStoringIssueQuantity)
                    .HasColumnName("product_storing_issue_quantity")
                    .HasDefaultValueSql("'0'")
                    .HasComment("入出庫数");

                entity.Property(e => e.ProductStoringIssueRemarks)
                    .HasMaxLength(255)
                    .HasColumnName("product_storing_issue_remarks")
                    .HasDefaultValueSql("'0'")
                    .HasComment("入出庫備考");

                entity.Property(e => e.RegistrationDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("registration_datetime")
                    .HasComment("登録日時");

                entity.Property(e => e.RegistrationUser)
                    .HasMaxLength(8)
                    .HasColumnName("registration_user")
                    .HasComment("登録者");

                entity.Property(e => e.UpdateDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("update_datetime")
                    .HasComment("更新日時");

                entity.Property(e => e.UpdateUser)
                    .HasMaxLength(8)
                    .HasColumnName("update_user")
                    .HasComment("更新者");
            });

            modelBuilder.Entity<TReception>(entity =>
            {
                entity.HasKey(e => new { e.StoreNumber, e.ReceptionNumber, e.ReceptionBranchNumber })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

                entity.ToTable("t_reception");

                entity.HasComment("受付テーブル")
                    .HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.StoreNumber)
                    .HasMaxLength(6)
                    .HasColumnName("store_number")
                    .IsFixedLength(true)
                    .HasComment("店舗番号");

                entity.Property(e => e.ReceptionNumber)
                    .HasMaxLength(11)
                    .HasColumnName("reception_number")
                    .IsFixedLength(true)
                    .HasComment("受付番号");

                entity.Property(e => e.ReceptionBranchNumber)
                    .HasColumnName("reception_branch_number")
                    .HasComment("受付枝番");

                entity.Property(e => e.CustomerNumber)
                    .HasMaxLength(8)
                    .HasColumnName("customer_number")
                    .IsFixedLength(true)
                    .HasComment("顧客コード");

                entity.Property(e => e.ReceptionDelFlg)
                    .HasMaxLength(1)
                    .HasColumnName("reception_del_flg")
                    .HasDefaultValueSql("'0'")
                    .IsFixedLength(true)
                    .HasComment("削除フラグ");

                entity.Property(e => e.ReceptionKbn)
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasColumnName("reception_kbn")
                    .IsFixedLength(true)
                    .HasComment("受付区分");

                entity.Property(e => e.ReceptionRemarks)
                    .HasMaxLength(255)
                    .HasColumnName("reception_remarks")
                    .HasComment("備考");

                entity.Property(e => e.RegistrationDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("registration_datetime")
                    .HasComment("登録日時");

                entity.Property(e => e.RegistrationUser)
                    .HasMaxLength(8)
                    .HasColumnName("registration_user")
                    .HasComment("登録者");

                entity.Property(e => e.SeatEndDate)
                    .HasColumnType("datetime")
                    .HasColumnName("seat_end_date")
                    .HasComment("退席日時");

                entity.Property(e => e.SeatLevel)
                    .HasMaxLength(1)
                    .HasColumnName("seat_level")
                    .IsFixedLength(true)
                    .HasComment("階層番号");

                entity.Property(e => e.SeatNumber)
                    .HasMaxLength(2)
                    .HasColumnName("seat_number")
                    .IsFixedLength(true)
                    .HasComment("座席番号");

                entity.Property(e => e.SeatPeopleChildren)
                    .HasColumnName("seat_people_children")
                    .HasDefaultValueSql("'0'")
                    .HasComment("子供人数");

                entity.Property(e => e.SeatPeopleMan)
                    .HasColumnName("seat_people_man")
                    .HasDefaultValueSql("'0'")
                    .HasComment("男性人数");

                entity.Property(e => e.SeatPeopleWoman)
                    .HasColumnName("seat_people_woman")
                    .HasDefaultValueSql("'0'")
                    .HasComment("女性人数");

                entity.Property(e => e.SeatStartDate)
                    .HasColumnType("datetime")
                    .HasColumnName("seat_start_date")
                    .HasComment("着席日時");

                entity.Property(e => e.SeatStatusKbn)
                    .HasMaxLength(3)
                    .HasColumnName("seat_status_kbn")
                    .IsFixedLength(true)
                    .HasComment("座席状況区分");

                entity.Property(e => e.TakeoutName)
                    .HasMaxLength(50)
                    .HasColumnName("takeout_name")
                    .HasComment("持帰り氏名");

                entity.Property(e => e.TakeoutReceiptTime)
                    .HasColumnType("datetime")
                    .HasColumnName("takeout_receipt_time")
                    .HasComment("持帰り受取予定日時");

                entity.Property(e => e.TakeoutTel)
                    .HasMaxLength(15)
                    .HasColumnName("takeout_tel")
                    .HasComment("持帰り電話番号");

                entity.Property(e => e.UpdateDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("update_datetime")
                    .HasComment("更新日時");

                entity.Property(e => e.UpdateUser)
                    .HasMaxLength(8)
                    .HasColumnName("update_user")
                    .HasComment("更新者");
            });

            modelBuilder.Entity<VEquipmentInventoryNow>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("v_equipment_inventory_now");

                entity.Property(e => e.EquipmentNumber)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("equipment_number")
                    .IsFixedLength(true)
                    .HasComment("備品番号")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.EquipmentStockQuantity)
                    .HasColumnName("equipment_stock_quantity")
                    .HasDefaultValueSql("'0'")
                    .HasComment("在庫数");

                entity.Property(e => e.StoreNumber)
                    .IsRequired()
                    .HasMaxLength(6)
                    .HasColumnName("store_number")
                    .IsFixedLength(true)
                    .HasComment("店舗番号")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.入庫).HasPrecision(27);

                entity.Property(e => e.前回棚卸日)
                    .HasMaxLength(8)
                    .IsFixedLength(true)
                    .HasComment("備品棚卸日")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.消費).HasPrecision(27);

                entity.Property(e => e.論理在庫数).HasPrecision(29);
            });

            modelBuilder.Entity<VMaterialInventoryNow>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("v_material_inventory_now");

                entity.Property(e => e.MaterialNumber)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("material_number")
                    .IsFixedLength(true)
                    .HasComment("原材料番号")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.MaterialStockQuantity)
                    .HasColumnName("material_stock_quantity")
                    .HasDefaultValueSql("'0'")
                    .HasComment("在庫数");

                entity.Property(e => e.StoreNumber)
                    .IsRequired()
                    .HasMaxLength(6)
                    .HasColumnName("store_number")
                    .IsFixedLength(true)
                    .HasComment("店舗番号")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.入庫).HasPrecision(27);

                entity.Property(e => e.前回棚卸日)
                    .HasMaxLength(8)
                    .IsFixedLength(true)
                    .HasComment("原材料棚卸日")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.消費).HasPrecision(27);

                entity.Property(e => e.論理在庫数).HasPrecision(29);
            });

            modelBuilder.Entity<VProductInventoryNow>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("v_product_inventory_now");

                entity.Property(e => e.ProductNumber)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("product_number")
                    .IsFixedLength(true)
                    .HasComment("商品番号")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.ProductStockQuantity)
                    .HasColumnName("product_stock_quantity")
                    .HasDefaultValueSql("'0'")
                    .HasComment("在庫数");

                entity.Property(e => e.StoreNumber)
                    .IsRequired()
                    .HasMaxLength(6)
                    .HasColumnName("store_number")
                    .IsFixedLength(true)
                    .HasComment("店舗番号")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.入庫).HasPrecision(27);

                entity.Property(e => e.前回棚卸日)
                    .HasMaxLength(8)
                    .IsFixedLength(true)
                    .HasComment("商品棚卸日")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.消費).HasPrecision(27);

                entity.Property(e => e.論理在庫数).HasPrecision(29);
            });

            modelBuilder.Entity<VProductLimitSeteach>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("v_product_limit_seteach");

                entity.Property(e => e.LimitedQuantity)
                    .HasPrecision(26)
                    .HasColumnName("limited_quantity");

                entity.Property(e => e.ProductNumber)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("product_number")
                    .IsFixedLength(true)
                    .HasComment("商品番号")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.ProductSetNumber)
                    .HasMaxLength(2)
                    .HasColumnName("product_set_number")
                    .IsFixedLength(true)
                    .HasComment("商品セット番号")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.StoreNumber)
                    .IsRequired()
                    .HasMaxLength(6)
                    .HasColumnName("store_number")
                    .IsFixedLength(true)
                    .HasComment("店舗番号")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.SumOrderQuantity)
                    .HasPrecision(25)
                    .HasColumnName("sum_order_quantity");

                entity.Property(e => e.SumProductLimitedQuantity)
                    .HasPrecision(25)
                    .HasColumnName("sum_product_limited_quantity");
            });

            modelBuilder.Entity<VProductLimitSetsum>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("v_product_limit_setsum");

                entity.Property(e => e.LimitedQuantity)
                    .HasPrecision(26)
                    .HasColumnName("limited_quantity");

                entity.Property(e => e.ProductNumber)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("product_number")
                    .IsFixedLength(true)
                    .HasComment("商品番号")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.StoreNumber)
                    .IsRequired()
                    .HasMaxLength(6)
                    .HasColumnName("store_number")
                    .IsFixedLength(true)
                    .HasComment("店舗番号")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.SumOrderQuantity)
                    .HasPrecision(25)
                    .HasColumnName("sum_order_quantity");

                entity.Property(e => e.SumProductLimitedQuantity)
                    .HasPrecision(25)
                    .HasColumnName("sum_product_limited_quantity");
            });

            modelBuilder.Entity<VProductMenuItem>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("v_product_menu_item");

                entity.Property(e => e.LimitedQuantity)
                    .HasPrecision(26)
                    .HasColumnName("limited_quantity");

                entity.Property(e => e.MenuClassKbnAbbr)
                    .HasMaxLength(50)
                    .HasColumnName("menu_class_kbn_abbr")
                    .HasComment("区分値略称")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.MenuClassKbnName)
                    .HasMaxLength(255)
                    .HasColumnName("menu_class_kbn_name")
                    .HasComment("区分値名称")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.ProductCalorie)
                    .HasColumnName("product_calorie")
                    .HasDefaultValueSql("'0'")
                    .HasComment("カロリー");

                entity.Property(e => e.ProductCookingtime)
                    .HasColumnName("product_cookingtime")
                    .HasDefaultValueSql("'0'")
                    .HasComment("調理目安時間");

                entity.Property(e => e.ProductCourseFlg)
                    .HasMaxLength(1)
                    .HasColumnName("product_course_flg")
                    .HasDefaultValueSql("'0'")
                    .IsFixedLength(true)
                    .HasComment("コース料理フラグ")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.ProductCourseOrder)
                    .HasColumnName("product_course_order")
                    .HasDefaultValueSql("'0'")
                    .HasComment("コース料理順");

                entity.Property(e => e.ProductImage)
                    .HasMaxLength(255)
                    .HasColumnName("product_image")
                    .HasComment("商品画像")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.ProductIntroduction)
                    .HasColumnType("text")
                    .HasColumnName("product_introduction")
                    .HasComment("商品紹介")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.ProductLimitedKbn)
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasColumnName("product_limited_kbn")
                    .IsFixedLength(true)
                    .HasComment("数量限定区分")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.ProductMenuClassKbn)
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasColumnName("product_menu_class_kbn")
                    .IsFixedLength(true)
                    .HasComment("商品メニュー分類区分")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.ProductMenuLineNumber)
                    .HasColumnName("product_menu_line_number")
                    .HasComment("商品メニュー分類表示順");

                entity.Property(e => e.ProductMenuNovisibleFlg)
                    .HasMaxLength(1)
                    .HasColumnName("product_menu_novisible_flg")
                    .HasDefaultValueSql("'0'")
                    .IsFixedLength(true)
                    .HasComment("商品メニュー非表示フラグ")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.ProductMenuNumber)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("product_menu_number")
                    .IsFixedLength(true)
                    .HasComment("商品メニュー管理番号")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.ProductMenuOrderNumber)
                    .IsRequired()
                    .HasMaxLength(4)
                    .HasColumnName("product_menu_order_number")
                    .IsFixedLength(true)
                    .HasComment("商品メニュー注文番号")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.ProductNameCh)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("product_name_ch")
                    .HasComment("商品名称（中国語）")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.ProductNameJp)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("product_name_jp")
                    .HasComment("商品名称（日本語）")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.ProductNumber)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("product_number")
                    .IsFixedLength(true)
                    .HasComment("商品番号")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.ProductOffertimeFrom)
                    .HasMaxLength(5)
                    .HasColumnName("product_offertime_from")
                    .IsFixedLength(true)
                    .HasComment("提供時間帯From")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.ProductOffertimeTo)
                    .HasMaxLength(5)
                    .HasColumnName("product_offertime_to")
                    .IsFixedLength(true)
                    .HasComment("提供時間帯To")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.ProductPicupFlg)
                    .HasMaxLength(1)
                    .HasColumnName("product_picup_flg")
                    .HasDefaultValueSql("'0'")
                    .IsFixedLength(true)
                    .HasComment("お薦めフラグ")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.ProductPreparationFlg)
                    .HasMaxLength(1)
                    .HasColumnName("product_preparation_flg")
                    .HasDefaultValueSql("'0'")
                    .IsFixedLength(true)
                    .HasComment("準備中フラグ")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.ProductPrice)
                    .HasColumnType("mediumint unsigned")
                    .HasColumnName("product_price")
                    .HasComment("価格（税込）");

                entity.Property(e => e.ProductRemarks)
                    .HasMaxLength(50)
                    .HasColumnName("product_remarks")
                    .HasComment("商品備考")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.ProductSetExistenceFlg)
                    .HasMaxLength(1)
                    .HasColumnName("product_set_existence_flg")
                    .HasDefaultValueSql("'0'")
                    .IsFixedLength(true)
                    .HasComment("セット品有無フラグ")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.ProductSetName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("product_set_name")
                    .HasComment("商品セット名称")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.ProductSetNumber)
                    .IsRequired()
                    .HasMaxLength(2)
                    .HasColumnName("product_set_number")
                    .IsFixedLength(true)
                    .HasComment("商品セット番号")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.ProductStokFlg)
                    .HasMaxLength(1)
                    .HasColumnName("product_stok_flg")
                    .HasDefaultValueSql("'0'")
                    .IsFixedLength(true)
                    .HasComment("在庫連携フラグ")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.ProductTaxKbn)
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasColumnName("product_tax_kbn")
                    .IsFixedLength(true)
                    .HasComment("税率区分")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.ProductTypeKbn)
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasColumnName("product_type_kbn")
                    .IsFixedLength(true)
                    .HasComment("商品分類区分")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.ProductUnitKbn)
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasColumnName("product_unit_kbn")
                    .IsFixedLength(true)
                    .HasComment("単位区分")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.StoreNumber)
                    .IsRequired()
                    .HasMaxLength(6)
                    .HasColumnName("store_number")
                    .IsFixedLength(true)
                    .HasComment("店舗番号")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.TypeKbnAbbr)
                    .HasMaxLength(50)
                    .HasColumnName("type_kbn_abbr")
                    .HasComment("区分値略称")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.TypeKbnName)
                    .HasMaxLength(255)
                    .HasColumnName("type_kbn_name")
                    .HasComment("区分値名称")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.UnitKbnAbbr)
                    .HasMaxLength(50)
                    .HasColumnName("unit_kbn_abbr")
                    .HasComment("区分値略称")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.UnitKbnName)
                    .HasMaxLength(255)
                    .HasColumnName("unit_kbn_name")
                    .HasComment("区分値名称")
                    .UseCollation("utf8_general_ci")
                    .HasCharSet("utf8");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
