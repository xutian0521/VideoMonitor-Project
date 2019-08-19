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
    public class SystemController : ControllerBasic
    {
        //
        // GET: /System/
        public ActionResult Index()
        {
            return View();
        }

        //获取权限菜单 集合json
        public async Task<ActionResult> GetSystemMenusAsync()
        {
            int userId = base.UserId;

            var bll = base.CreateBusiness<ISystemBusiness>();
            var menus = await bll.GetPermissionMenusByUserId(userId);

            return Json(menus, JsonRequestBehavior.AllowGet);
        }
    }
}