using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoMonitor.Entities.ViewModel
{
    public class VideoViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }

        public int Sort { get; set; }
        public int CreateUserId { get; set; }
        public bool isDeleted { get; set; }
        public DateTime CreateTime { get; set; }
        public string Remark { get; set; }
        public string BoxNum { get; set; }

        public string SubType { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }

        public string CompanyName { get; set; }
        public string Phone { get; set; }
        public string Location { get; set; }
        public string Salesman { get; set; }
        public int ParentId { get; set; }

    }
}
