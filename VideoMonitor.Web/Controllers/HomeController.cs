using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VideoMonitor.Business;
using VideoMonitor.Business.Interface;
using VideoMonitor.Entities.Models;
using VideoMonitor.Entities.ViewModel;
using VideoMonitor.Web.Controllers.Base;

namespace VideoMonitor.Web.Controllers
{
    public class HomeController : ControllerBasic
    {

        public async Task<ActionResult> Index()
        {
            var bll = base.CreateBusiness<IUserBusiness>();
            var authBll = new AuthBusiness();

            var user_ = await bll.GetUserViewModelAsync(base.UserId);
            ViewBag.UserName = user_.UserName;
            ViewBag.RoleName = user_.RoleName;

            var userType =await authBll.GetUserTypeAsync(base.UserId);
            if (userType ==Entities.Enums.UserType.Customer)
            {
                return RedirectToAction("ValidateRecorder", "Video");
            }
            else
            {
                return View();
            }
            
        }
        //获取权限菜单 json数据
        public async Task<ActionResult> GetClientsDataJson()
        {
            int userId = base.UserId;

            var bll = base.CreateBusiness<ISystemBusiness>();

            var authorizeMenu = await bll.GetPermissionMenusByUserId(userId);
            var data = new
            {
                dataItems = "",
                organize = "",
                role = "",
                duty = "",
                user = "",
                authorizeMenu =this.ToMenusViewModel(authorizeMenu),
                authorizeButton = "",
            };

            return Json(data,JsonRequestBehavior.AllowGet);
        }

        //把权限list集合转换为 json数据
        private List<MenuViewModel> ToMenusViewModel(List<Permission> list)
        {
            var data= list.Select(p => new MenuViewModel
            {
                Id = p.Id,
                ActionName = p.ActionName,
                ControllerName = p.ControllerName,
                CreateTime = p.CreateTime,
                FormMethod = p.FormMethod,
                Ico = p.Ico,
                isDeleted = p.isDeleted,
                IsShow = p.IsShow,
                LinkUrl = p.LinkUrl,
                OperationType = p.OperationType,
                ParentId = p.ParentId,
                PermissionName = p.PermissionName,
                Remark = p.Remark,

            }).ToList();

            List<MenuViewModel> result = new List<MenuViewModel>();
            foreach (var item in data)
            {
                var parent = data.Where(t => t.Id == item.ParentId && item.ParentId !=0).FirstOrDefault();
                if (parent !=null)
                {
                    if (parent.ChildNodes ==null)
                    {
                        parent.ChildNodes = new List<MenuViewModel>();
                    }

                    parent.ChildNodes.Add(item);

                    if(!result.Contains(parent))
                    {
                        result.Add(parent);
                    }
                }
            }

            return result;
        }
        public ActionResult About()
        {

            //var mNum= Common.PlatformAuthorizeHelper.GetMachineCode();
            //var rNum = Common.PlatformAuthorizeHelper.GetPollCode(mNum,new TimeSpan(1,0,0,0));
            //var ss = Common.EncryptUtil.UnBase64Variant(rNum);
            //var code = Common.PlatformAuthorizeHelper.CheckPlatformIsAuthorize();
            //ViewBag.Message =string.Format( "机器码：{0}---- 注册码{1}：",mNum,rNum);
            //Common.PlatformAuthorizeHelper.WriteAuthorizeToConfig(rNum);

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}