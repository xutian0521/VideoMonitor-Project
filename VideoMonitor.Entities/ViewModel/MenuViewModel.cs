using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoMonitor.Entities.ViewModel
{
    public class MenuViewModel
    {
        public int Id { get; set; }
        public string PermissionName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public short FormMethod { get; set; }
        public short OperationType { get; set; }
        public string Ico { get; set; }
        public string LinkUrl { get; set; }
        public bool IsShow { get; set; }
        public int ParentId { get; set; }
        public bool isDeleted { get; set; }
        public DateTime CreateTime { get; set; }
        public string Remark { get; set; }

        public List<MenuViewModel> ChildNodes { get; set; }

    }
}
