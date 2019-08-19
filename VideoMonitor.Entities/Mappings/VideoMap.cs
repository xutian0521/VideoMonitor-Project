using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoMonitor.Entities.Models;

namespace VideoMonitor.Entities.Mappings
{
    public class VideoMap : EntityTypeConfiguration<Video>
    {
        public VideoMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("Videos");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Path).HasColumnName("Path");
            this.Property(t => t.Sort).HasColumnName("Sort");
            this.Property(t => t.isDeleted).HasColumnName("isDeleted");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.CreateUserId).HasColumnName("CreateUserId");
            this.Property(t => t.VideoType).HasColumnName("VideoType");
            this.Property(t => t.BoxNum).HasColumnName("BoxNum");
            this.Property(t => t.Location).HasColumnName("Location");
            this.Property(t => t.Lng).HasColumnName("Lng");
            this.Property(t => t.Lat).HasColumnName("Lat");
            this.HasRequired(t => t.User)
                .WithMany(t => t.Videos).HasForeignKey(t => t.CreateUserId);
            this.HasRequired(t => t.DataDictionary).WithMany(t => t.Videos).HasForeignKey(t => t.SubType);

        }
    }
}
