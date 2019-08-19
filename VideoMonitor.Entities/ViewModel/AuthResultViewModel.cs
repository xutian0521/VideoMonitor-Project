using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoMonitor.Entities.Enums;

namespace VideoMonitor.Entities.ViewModel
{
    public class LoginResultViewModel
    {
        public bool IsSuccess { get; set; }

        public string MessageResult { get; set; }

        public UserType UserType { get; set; }
        public int UserId { get; set; }

        public string NextUrl { get; set; }
    }
}
