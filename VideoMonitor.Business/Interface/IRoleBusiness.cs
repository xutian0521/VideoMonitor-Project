using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoMonitor.Entities.Models;

namespace VideoMonitor.Business.Interface
{
    public interface IRoleBusiness
    {
        /// <summary>
        /// 根据用户id 获取自己角色下 应有的角色集合
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<Role>> GetRoleListAsync(int userId);

        /// <summary>
        /// 获取所以角色 的集合
        /// </summary>
        /// <returns></returns>
        Task<List<Role>> GetAllRoleListAsync();
    }
}
