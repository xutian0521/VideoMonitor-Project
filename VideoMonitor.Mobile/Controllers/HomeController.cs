using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VideoMonitor.Mobile.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult UploadVideo()
        {
            HttpPostedFileBase files = Request.Files["video-blob"];
            if (files != null && files.ContentLength > 0)
            {
                string folderpath = "/UploadFile/Video/";
                if (!Directory.Exists(folderpath))
                {
                    Directory.CreateDirectory(Server.MapPath(folderpath));
                }
                string ext1 = Path.GetExtension(files.FileName);
                if (ext1 != ".mp4" && ext1 != ".webm" && ext1 != ".mpeg" && ext1 != ".avi" && ext1 != ".mp3" && ext1 != ".wav" && ext1 != ".wma" && ext1 != ".rmvb")
                {
                    return Json(new { sta = false, msg = "文件格式不正确！" });
                }
                else
                {
                    string name = DateTime.Now.ToString("yyyyMMddHHmmssff");
                    string ext = Path.GetExtension(files.FileName);
                    string downpath = folderpath + name + ext;
                    string filepath = Server.MapPath(folderpath) + name + ext;
                    files.SaveAs(filepath);
                    return Json(new { sta = true, previewSrc = downpath, id = name });
                }
            }
            else
            {
                return Json(new { sta = false, msg = "请上传文件！" });
            }
        }
    }
}