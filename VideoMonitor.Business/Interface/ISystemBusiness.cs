using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoMonitor.Entities.Models;

namespace VideoMonitor.Business.Interface
{
    public interface ISystemBusiness
    {
        /// <summary>
        /// 获取该用户 权限菜单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<Permission>> GetPermissionMenusByUserId(int userId);
    }
}
