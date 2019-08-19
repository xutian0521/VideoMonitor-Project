using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoMonitor.Business.Infrastructure;
using VideoMonitor.Entities.Models;
using System.Data.Entity;
using VideoMonitor.Business.Interface;

namespace VideoMonitor.Business
{
    public class SystemBusiness : BusinessBase, ISystemBusiness
    {
        /// <summary>
        /// 获取该用户 权限菜单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<Permission>> GetPermissionMenusByUserId(int userId)
        {
            var permissiondb= base.CreateDb<Permission>();
            var rolePermissionDb=base.CreateDb<RolePermission>();
            var userRoleDb=base.CreateDb<UserRole>();


            var listUserRoles=await userRoleDb.Where(u => u.UserId == userId).Include(u=>u.Role).ToListAsync();

            var listAllRoles = new List<Role>();

            foreach (var item in listUserRoles)
            {
                listAllRoles.Add(item.Role);
            }

            var listAllRoleIds = listAllRoles.Select(t => t.Id).ToList();
            var ListAllRolePermission=await rolePermissionDb.Where(p => listAllRoleIds.Contains(p.RoleId)).ToListAsync();

            var ListAllRolePermissionIds = ListAllRolePermission.Select(t => t.PermissionId).ToList();
            var AllPermissionMenus=await permissiondb.Where(p => ListAllRolePermissionIds.Contains(p.Id)).ToListAsync();


            return AllPermissionMenus;
        }
    }
}
