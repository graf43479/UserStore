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
    public class ExceptionDetailConfig : EntityTypeConfiguration<ExceptionDetail>
    {
        public ExceptionDetailConfig()
        {
            //ToTable("Prod");
            //HasKey(p => p.Id);
            //Property(p => p.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            //Property(p => p.Date).HasColumnType("datetime2");
        }
    }
}
