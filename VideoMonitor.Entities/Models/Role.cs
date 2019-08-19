using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoMonitor.Entities.Infrastructure;

namespace VideoMonitor.Entities.Models
{
    public class Role: EntityBase
    {
        public Role()
        {
            this.RolePermissions = new List<RolePermission>();
            this.UserRoles = new List<UserRole>();
        }

        public int Id { get; set; }
        public string RoleName { get; set; }


        public virtual ICollection<RolePermission> RolePermissions { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
