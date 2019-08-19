using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VideoMonitor.Business.Interface;
using VideoMonitor.Common;
using VideoMonitor.Common.Attributes;
using VideoMonitor.Entities.ViewModel;
using VideoMonitor.Web.Controllers.Base;

namespace VideoMonitor.Web.Controllers
{
    public class AuthorizeController : ControllerBasic
    {
        [SkipLogonValidated]
        // GET: Authorize 授权页面
        public ActionResult Register()
        {
            PlatformAuthorizeHelper helper = new PlatformAuthorizeHelper();
            AuthorizeVM model = new AuthorizeVM();


            model.MachineCode = helper.GetMachineCode();
#if DEBUG
            model.AuthorizeCode = helper.GetAuthorizeCode(model.MachineCode, new TimeSpan(354, 0, 0, 0, 0));
#endif
            return View(model);
        }

        //授权请求action
        [SkipLogonValidated]
        public ActionResult Auth(AuthorizeVM model)
        {
            var isAuthorizeSuccess = false;
            DateTime? Deadline = null;
            var bll= base.CreateBusiness<IAuthBusiness>();

            if(ModelState.IsValid)
            {
                isAuthorizeSuccess= bll.AuthorizePlat(model);//调用业务层 验证授权码
                AuthorizeResultVM authResult = new AuthorizeResultVM();

                authResult.isAuthorizeSuccess = isAuthorizeSuccess;

                if (isAuthorizeSuccess)
                {
                    PlatformAuthorizeHelper helper = new PlatformAuthorizeHelper();

                    Deadline = helper.GetAuthorizedDeadline(model.AuthorizeCode);
                    authResult.AuthorizeDeadline = Deadline;

                    return View("AuthorizeResult" ,authResult);
                }
                else
                {

                    return View("AuthorizeResult", authResult);
                }
            }
            else
            {
                //todo:等父类 加了warning 提示，把error 换成warning
                return base.Error("未通过验证");
            }
        }

        //授权结果页面
        [SkipLogonValidated]
        public ActionResult AuthorizeResult(AuthorizeResultVM authResult)
        {
            return View(authResult);
        }
    }
}