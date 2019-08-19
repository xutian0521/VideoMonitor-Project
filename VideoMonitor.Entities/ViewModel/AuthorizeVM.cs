using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoMonitor.Entities.ViewModel
{
    public class AuthorizeVM
    {
        [Required]
        [DisplayName("平台识别码")]
        public string MachineCode { get; set; }
        [Required]
        [DisplayName("授权码")]
        public string AuthorizeCode { get; set; }
    }
}
