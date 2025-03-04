using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using cloudmenu_api.DbContextCloudMenu;
using cloudmenu_api.DbEntitiesCloudMenu;
using cloudmenu_api.DbentitiesExt;
using cloudmenu_api.DbEntitiesExt;
using cloudmenu_api.DailyReport;

#nullable disable

namespace cloudmenu_api.DbContextCloudMenu
{
    public partial class AppDbContext : DbContext
    {

        public virtual DbSet<DbExtSeatReception> ExtSeatReceptions { get; set; }
        public virtual DbSet<DbExtMenuReception> ExtMenuReception { get; set; }
        public virtual DbSet<DbExtBChoiceReception> ExtBChoiceReception { get; set; }
        public virtual DbSet<DbExOrderNumReception> ExOrderNumReception { get; set; }
        public virtual DbSet<DbExtStoring> ExtStoring { get; set; }
        public virtual DbSet<DbExtInventory> ExtInventory { get; set; }
        public virtual DbSet<DbExtTypeName> ExtTypeName { get; set; }
        public virtual DbSet<TEquipmentStoringIssue> EquipmentStoringIssue { get; set; }
        public virtual DbSet<TMaterialStoringIssue> MaterialStoringIssue { get; set; }
        public virtual DbSet<TProductStoringIssue> ProductStoringIssue { get; set; }

        public virtual DbSet<DbExtShohinType> ExtShohinType { get; set; }
        public virtual DbSet<TProductInventory> ProductInventory { get; set; }
        public virtual DbSet<TProductConsumption> ProductConsumption { get; set; }
        public virtual DbSet<TMaterialInventory> MaterialInventory { get; set; }
        public virtual DbSet<TMaterialConsumption> MaterialConsumption { get; set; }
        public virtual DbSet<TEquipmentInventory> EquipmentInventory { get; set; }
        public virtual DbSet<TEquipmentConsumption> EquipmentConsumption { get; set; }
        public virtual DbSet<DbExtStoringTime> ExtStoringTime { get; set; }
        public virtual DbSet<DbExtReceptionNumber> ExtReceptionNumber { get; set; }

        public virtual DbSet<CancelPrintReport> ExtCancelOrder { get; set; }
        public virtual DbSet<DBUser> ExtUser { get; set; }

        public virtual DbSet<DBTCancelOrder> ExtTCancelOrder { get; set; }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbExtSeatReception>().HasNoKey();
            modelBuilder.Entity<DbExtMenuReception>().HasNoKey();
            modelBuilder.Entity<DbExtBChoiceReception>().HasNoKey();
            modelBuilder.Entity<DbExOrderNumReception>().HasNoKey();
            modelBuilder.Entity<DbExtStoring>().HasNoKey();
            modelBuilder.Entity<DbExtInventory>().HasNoKey();
            modelBuilder.Entity<DbExtTypeName>().HasNoKey();
            modelBuilder.Entity<DbExtShohinType>().HasNoKey();
            modelBuilder.Entity<DbExtStoringTime>().HasNoKey();
            modelBuilder.Entity<DbExtReceptionNumber>().HasNoKey();
            modelBuilder.Entity<CancelPrintReport>().HasNoKey();
            modelBuilder.Entity<DBUser>().HasNoKey();
            modelBuilder.Entity<DBTCancelOrder>().HasNoKey();
        }
    }
}
