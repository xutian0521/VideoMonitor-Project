using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoMonitor.Entities.Models;

namespace VideoMonitor.Entities.ViewModel
{
    public class UserlayersVM
    {
        public User Self { get; set; }
        public List<UserlayersVM> ChildAccounts { get; set; }

        public int ParentId { get; set; }
    }
}
