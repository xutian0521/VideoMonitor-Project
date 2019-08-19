using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoMonitor.Business.Infrastructure;
using VideoMonitor.Business.Interface;
using VideoMonitor.Entities.Models;

namespace VideoMonitor.Business
{
    public class RoleBusiness :BusinessBase ,IRoleBusiness
    {
        /// <summary>
        /// 根据用户id 获取自己角色下 应有的角色集合
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<Role>> GetRoleListAsync(int userId)
        {

            var userRoleDb = base.CreateDb<UserRole>();

            var userRole_=await userRoleDb.Query().Where(t => t.UserId == userId).Include(t=>t.Role).FirstOrDefaultAsync();
            if (userRole_ ==null)
            {
                return null;
            }

            var roleDb = base.CreateDb<Role>();
            var query = roleDb.Query().Where(t => t.isDeleted == false);

            switch (userRole_.Role.RoleName)
            {
                case "管理员":
                    query = query.Where(t => t.Id != 3);//排除客户

                    break;
                case "业务员":
                    query = query.Where(t => t.Id != 1 && t.Id !=2);//排除管理员 和业务员
                    break;
                default:
                    query = query.Where(t => t.Id != 1 && t.Id != 2 && t.Id != 3); //默认排除 管理员，业务员 ，客户
                    break;

            }

            var list =await query.ToListAsync();
            return list;
        }

        /// <summary>
        /// 获取所以角色 的集合
        /// </summary>
        /// <returns></returns>
        public async Task<List<Role>> GetAllRoleListAsync()
        {
            var roleDb = base.CreateDb<Role>();

            var list = await roleDb.Query().ToListAsync();
            return list;
        }
    }
}
