using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VideoMonitor.Business;
using VideoMonitor.Common.Attributes;

namespace VideoMonitor.Web.Filters
{
    public class LoginValidateAttribute: AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            
            var authBll = new AuthBusiness();
            if (filterContext.ActionDescriptor.IsDefined(typeof(ValidateAuthorizeAttribute), false))
            {
                if(!authBll.CheckPlatformIsAuthorize())
                {
                    filterContext.Result = new RedirectResult("/Authorize/Register");
                }
            }
            if (!filterContext.ActionDescriptor.IsDefined(typeof(SkipLogonValidatedAttribute),false) 
                && !filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(SkipLogonValidatedAttribute),false))
            {
                if (!authBll.CheckIsLogin())
                {
                    filterContext.Result = new RedirectResult("/user/login");
                }
            }

        }
    }
}