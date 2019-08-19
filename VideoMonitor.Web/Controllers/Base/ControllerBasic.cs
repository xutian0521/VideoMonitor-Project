using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VideoMonitor.Business.Infrastructure;
using VideoMonitor.Common;
using VideoMonitor.Entities.ViewModel;

namespace VideoMonitor.Web.Controllers.Base
{
    public class ControllerBasic : Controller
    {
        // GET: ControllerBase
        protected T CreateBusiness<T>()
        {
            IKernel kernal = new StandardKernel(new BusinessModule());
            //Samurai s = new Samurai(kernal.Get<IWeapon>()); // 构造函数注入
            var s = kernal.Get<T>();
            return s;
        }
        /// <summary>
        /// 返回成功消息 的json
        /// </summary>
        /// <param name="message">要显示的消息</param>
        /// <returns></returns>
        protected virtual ActionResult Success(string message)
        {
            return Content(new AjaxResult { state = ResultType.success.ToString(), message = message }.ToJson());
        }

        /// <summary>
        /// 返回失败消息 的json
        /// </summary>
        /// <param name="message">要显示的消息</param>
        /// <returns></returns>
        protected virtual ActionResult Error(string message)
        {
            return Content(new AjaxResult { state = ResultType.error.ToString(), message = message }.ToJson());
        }
        //todo: 父类中加入返回 warning 警告提示
        /// <summary>
        /// 从当前请求cookie中 获取 userId
        /// </summary>
        protected int UserId 
        { 
            get 
            {
                int id_ = 0;
                int.TryParse(Request.Cookies.Get("userId").Value, out id_);
                return id_;
            } 
        }
    }
}