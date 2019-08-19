using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoMonitor.Entities.Models;

namespace VideoMonitor.Entities.Mappings
{
    public class DataDictionaryMap: EntityTypeConfiguration<DataDictionary>
    {
        public DataDictionaryMap()
        {
            this.HasKey(t => t.Id);

            this.ToTable("DataDictionaries");
            this.Property(t => t.TypeName).HasColumnName("TypeName");
            this.Property(t => t.TypeValue).HasColumnName("TypeValue");
            this.Property(t => t.Sort).HasColumnName("Sort");
            this.Property(t => t.ParentId).HasColumnName("ParentId");
            this.Property(t => t.isDeleted).HasColumnName("isDeleted");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.Remark).HasColumnName("Remark");
        }
    }
}
