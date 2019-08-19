using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoMonitor.Entities.Infrastructure
{
    public class EntityBase
    {
        public bool isDeleted { get; set; }
        public DateTime CreateTime { get; set; }
        [DisplayName("备注")]
        public string Remark { get; set; }
        
    }
}
