using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoMonitor.Entities.Infrastructure;

namespace VideoMonitor.Entities.Models
{
    public class DataDictionary: EntityBase
    {
        public int Id { get; set; }

        public string TypeName { get; set; }
        public string TypeValue { get; set; }

        public int Sort { get; set; }

        public int ParentId { get; set; }

        public ICollection<Video> Videos { get; set; }
    }
}
