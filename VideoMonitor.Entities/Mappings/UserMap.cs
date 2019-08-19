using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoMonitor.Entities.Models;

namespace VideoMonitor.Entities.Mappings
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("Users");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RealName).HasColumnName("RealName");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.PassWord).HasColumnName("PassWord");
            this.Property(t => t.Phone).HasColumnName("Phone");
            this.Property(t => t.CompanyName).HasColumnName("CompanyName");
            this.Property(t => t.isDeleted).HasColumnName("isDeleted");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.ParentId).HasColumnName("ParentId");

        }
    }
}
