using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VideoMonitor.Business;
using VideoMonitor.Business.Interface;
using VideoMonitor.Web.Controllers.Base;

namespace VideoMonitor.Web.Controllers
{
    public class RoleController : ControllerBasic
    {
        // GET: Role
        public ActionResult Index()
        {
            return View();
        }

        //获取当前用户权限下所有角色 集合的json
        public async Task<ActionResult> GetRoleJsonListAsync()
        {
            int userId = base.UserId;

            var bll = base.CreateBusiness<IRoleBusiness>();
            var data = await bll.GetRoleListAsync(userId);

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //获取所有角色 集合的json
        public async Task<ActionResult> GetAllRoleJsonListAsync()
        {
            int userId = base.UserId;

            var bll = base.CreateBusiness<IRoleBusiness>();
            var data = await bll.GetAllRoleListAsync();

            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}