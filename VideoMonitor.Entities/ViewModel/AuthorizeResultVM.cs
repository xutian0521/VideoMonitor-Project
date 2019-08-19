using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoMonitor.Entities.ViewModel
{
    /// <summary>
    /// 用户点击验证授权后 返回的验证信息
    /// </summary>
    public class AuthorizeResultVM
    {
        /// <summary>
        /// 是否授权成功
        /// </summary>
        public bool isAuthorizeSuccess { get; set; }
        /// <summary>
        /// 授权截止时间
        /// </summary>
        public DateTime? AuthorizeDeadline { get; set; }

    }
}
