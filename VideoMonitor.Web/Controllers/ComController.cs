using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VideoMonitor.Common;

namespace VideoMonitor.Web.Controllers
{
    public class ComController: Controller
    {
        //检查箱号是否正确
        public ActionResult CheckBoxNumIsRight(string boxNum)
        {
            if (!string.IsNullOrEmpty(boxNum) && ValidateHelper.CheckContainerNumber(boxNum))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetCityName()
        {
            var ipInfo = IPHelper.IPGetCity();
            var city = "";
            if (ipInfo != null)
            {
                city = ipInfo["city"].ToString();
            }
            return Json(city, JsonRequestBehavior.AllowGet);
        }
    }
}