using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoMonitor.Entities.Models;

namespace VideoMonitor.Entities.Mappings
{
    public class PermissionMap : EntityTypeConfiguration<Permission>
    {
        public PermissionMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("Permissions");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PermissionName).HasColumnName("PermissionName");
            this.Property(t => t.ControllerName).HasColumnName("ControllerName");
            this.Property(t => t.ActionName).HasColumnName("ActionName");
            this.Property(t => t.FormMethod).HasColumnName("FormMethod");
            this.Property(t => t.OperationType).HasColumnName("OperationType");
            this.Property(t => t.Ico).HasColumnName("Ico");
            this.Property(t => t.LinkUrl).HasColumnName("LinkUrl");
            this.Property(t => t.IsShow).HasColumnName("IsShow");
            this.Property(t => t.isDeleted).HasColumnName("isDeleted");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.Remark).HasColumnName("Remark");
        }
    }
}
