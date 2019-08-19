using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VideoMonitor.Business;
using VideoMonitor.Business.Interface;
using VideoMonitor.Common;
using VideoMonitor.Entities.Enums;
using VideoMonitor.Entities.ViewModel;
using VideoMonitor.Web.Controllers.Base;

namespace VideoMonitor.Web.Controllers
{
    public class VideoController : ControllerBasic
    {

        //获取视频 列表集合
        public async Task<ActionResult> GetVideoListJsonAsync()
        {
            int userId = base.UserId;

            var videoBll = base.CreateBusiness<IVideoBusiness>();
            var list = await videoBll.GetVideosByUserIdAsync(userId);

            return Json(list,JsonRequestBehavior.AllowGet);
        }

        //获取视频 分页的列表集合
        public async Task<ActionResult> GetVideoPagedListJsonAsync(Pagination parm, VideoType type,string keyWords)
        {
            
            int userId = base.UserId;

            var videoBll = base.CreateBusiness<IVideoBusiness>();
            var pagedList = await videoBll.GetPagedVideosByUserIdAsync(parm, userId, (int)type, keyWords);

            return Json(pagedList, JsonRequestBehavior.AllowGet);
        }

        //删除视频
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var videoBll = base.CreateBusiness<IVideoBusiness>();

            var row = await videoBll.DeleteByIdAsync(id);
            if (row > 0)
            {
                return base.Success("删除成功！");
            }
            else
            {
                return base.Error("删除失败！");
            }
        }

        //查看视频
        public async Task<ActionResult> Watch(int id)
        {
            var videoBll = base.CreateBusiness<IVideoBusiness>();

            var video = await videoBll.GetVideoByIdAsync(id);
            return View(video);
        }

        //查看地图
        public async Task<ActionResult> Map(int id)
        {
            var videoBll = base.CreateBusiness<IVideoBusiness>();

            var video = await videoBll.GetVideoByIdAsync(id);
            MapInfo info = new MapInfo();
            info.Location = video.Location;
            info.Lat = video.Lat;
            info.Lng = video.Lng;

            return View(info);
        }
        /// <summary>
        /// 上传视频的action
        /// </summary>
        /// <param name="type">视频类型</param>
        /// <param name="boxNum">箱号</param>
        /// <param name="subType">子类型</param>
        /// <param name="location">所在位置</param>
        /// <returns></returns>
        public async Task<ActionResult> UploadVideoAsync(VideoType type,int subType, string boxNum, string location,double lng,double lat)
        {
            
            HttpPostedFileBase files = Request.Files["video-blob"];


            if (files != null && files.ContentLength > 0)//浏览器上传的文件
            {
                string folderpath;
                switch (type)//不同的视频分别存放在不同的文件夹中
                {
                    case VideoType.Monitor:
                        folderpath = "/UploadFile/MonitorVideo/";
                        break;
                    case VideoType.Validate:
                        folderpath = "/UploadFile/ValidateVideo/";
                        break;
                    default:
                        folderpath = "/UploadFile/Video/";
                        break;
                }
                if (!Directory.Exists(folderpath))
                {
                    Directory.CreateDirectory(Server.MapPath(folderpath));
                }

                string ext1 = Path.GetExtension(files.FileName).ToLower();
                if (ext1 != ".mp4" && ext1 != ".webm" && ext1 != ".mpeg" && ext1 != ".avi" && ext1 != ".mp3" && ext1 != ".wav" && ext1 != ".wma" && ext1 != ".rmvb" && ext1 != ".mov")
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
                    var videoBll = base.CreateBusiness<IVideoBusiness>();
                    if (boxNum == "undefined")//如果没箱号
                    {
                        boxNum = "";
                    }

                    var row = await videoBll.AddVideoAsync(type, subType, base.UserId, downpath, boxNum, location, lng, lat);
                    return Json(new { sta = true, previewSrc = downpath, id = name,row = row });
                }
            }
            else
            {
                return Json(new { sta = false, msg = "请上传文件！" });
            }
        }


        //验厂列表
        public ActionResult Validate()
        {
            ViewBag.VideoType = (int)VideoType.Validate;
            return View("List");
        }

        //监装列表
        public ActionResult Monitor()
        {
            ViewBag.VideoType = (int)VideoType.Monitor;
            return View("List");
        }
        //签约列表
        public ActionResult Contract()
        {
            ViewBag.VideoType = (int)VideoType.Contract;
            return View("List");
        }
        //验货列表
        public ActionResult InspectGoods()
        {
            ViewBag.VideoType = (int)VideoType.InspectGoods;
            return View("List");
        }

        //验厂录制   --mobile
        public ActionResult ValidateRecorder()
        {
            return View();
        }

        //监装录制  --mobile
        public ActionResult MonitorRecorder()
        {
            return View();
        }
        //签约录制  --mobile
        public ActionResult ContractRecorder()
        {
            return View();
        }
        //验货录制  --mobile
        public ActionResult InspectGoodsRecorder()
        {
            return View();
        }
    }
}