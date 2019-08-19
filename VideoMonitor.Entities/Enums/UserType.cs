using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoMonitor.Entities.Enums
{
    /// <summary>
    /// 用户类型(角色身份)
    /// </summary>
    public enum UserType
    {
        /// <summary>
        /// 无角色
        /// </summary>
        None = 0,
        /// <summary>
        /// 管理员
        /// </summary>
        Admin = 1,
        /// <summary>
        /// 业务员
        /// </summary>
        Salesman = 2,
        /// <summary>
        /// 客户
        /// </summary>
        Customer = 3,
    }
}
