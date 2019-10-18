using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStore.DAL.Entities;

namespace UserStore.DAL.Configurations
{
    class ProductConfig : EntityTypeConfiguration<Product>
    {
        public ProductConfig()
        {
            //ToTable("Prod");
            HasKey(p => p.ProductID);
            Property(p => p.ProductID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            //Property(p => p.Code).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(p => p.Name).IsRequired();
            //Property(p => p.CurrentPrice).IsRequired();
            //Property(p => p.Url).IsOptional();
            //Property(p => p.CreationDate).HasColumnType("date");
            //Property(p => p.UpdateDate).HasColumnType("date");
            //HasMany(x => x.Prices).WithRequired(x => x.Product).HasForeignKey(x => x.ProductID);
            //Property(p => p.CreationDate).HasColumnType("datetime2");
            //HasMany(x => x.Prices).WithRequired(x => x.Statistic).HasForeignKey(x => x.StatisticID);
            //HasMany(x => x.Products).WithRequired(x => x.Vendor).HasForeignKey(x => x.VendorID);
        }
    }
}
