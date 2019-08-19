using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoMonitor.Entities.ViewModel
{
    public class UserInfoViewModel
    {
        public int Id { get; set; }
        public string RealName { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string Phone { get; set; }
        public string CompanyName { get; set; }
        public int ParentId { get; set; }
        public string Remark { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }


        public string SalesmanName { get; set; }
    }
}
