using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoMonitor.Business.Infrastructure;
using VideoMonitor.Business.Interface;
using VideoMonitor.Common;
using VideoMonitor.Entities.Enums;
using VideoMonitor.Entities.Models;
using VideoMonitor.Entities.ViewModel;

namespace VideoMonitor.Business
{
    public class VideoBusiness: BusinessBase, IVideoBusiness
    {
        /// <summary>
        /// 根据id获取 第一个video
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Video> GetVideoByIdAsync(int id)
        {
            var userBll = new UserBusiness();
            var db = base.CreateDb<Video>();

            var video = await db.Where(t => t.Id == id).FirstOrDefaultAsync();
            return video;
        }

        /// <summary>
        /// 根据 userId获取该用户下和子用户下的所有视频集合
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<Video>> GetVideosByUserIdAsync(int userId)
        {
            var userBll = new UserBusiness();

            var db = base.CreateDb<Video>();
            var allUserId = await userBll.GetAllUserIdAsync(userId);

            var list=await db.Where(t => allUserId.Contains(t.CreateUserId)).ToListAsync();
            return list;
        }

        /// <summary>
        /// 根据 userId获取该用户下和子用户下的所有视频 分页的集合
        /// </summary>
        /// <param name="parm">分页参数</param>
        /// <param name="userId">用户id</param>
        /// <param name="videoType">视频类型</param>
        /// <param name="keyWords">关键字</param>
        /// <returns>分页的视频viewmodel集合</returns>
        public async Task<Paged<VideoViewModel>> GetPagedVideosByUserIdAsync(Pagination parm, int userId, int videoType, string keyWords)
        {
            var userBll = new UserBusiness();

            var db = base.CreateDb<Video>();
            var allUserId = await userBll.GetAllUserIdAsync(userId);

            var query = db.Where(t => allUserId.Contains(t.CreateUserId) && t.VideoType == videoType);
            if (!string.IsNullOrEmpty(keyWords))
            {
                query = query.Where(t => t.User.CompanyName.Contains(keyWords) || t.User.Phone.Contains(keyWords) || t.User.UserName.Contains(keyWords) || t.User.RealName.Contains(keyWords));
            }

            var paged =await query.Include(t=>t.User.UserRoles.Select(r=>r.Role)).Include(t=>t.DataDictionary).ToPagedListAsync(parm, t => t.CreateTime);

            var viewModelList = new List<VideoViewModel>();

            var pagedViewModel = new Paged<VideoViewModel>()
            {

                PageIndex = paged.PageIndex,
                PageSize = paged.PageSize,
                Total = paged.Total,
                Data = viewModelList
            };
            var listUser =await userBll.GetAllUserListAsync();//获取所有用户list
            

            foreach (var item in paged.Data)
            {
                var _viewModel = item.ToViewModel();
                var _user= listUser.Where(t => t.Id == _viewModel.ParentId).FirstOrDefault();
                if (_user != null)//如果该视频创建者 用户有父级用户
                {
                    if (_user.ParentId != 0)//并且父级用户还有父级用户，说明该用户是业务员
                    {
                        _viewModel.Salesman = _user.RealName;
                    }
                }
 
                viewModelList.Add(_viewModel);
            }

            return pagedViewModel;
        }
        /// <summary>
        /// 根据id 删除视频
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteByIdAsync(int id)
        {
            var db = base.CreateDb<Video>();

            var video = new Video();
            video.Id = id;

            db.Delete(video);

            var row = await db.SaveChangesAsync();
            return row;
        }


        /// <summary>
        /// 新增视频
        /// </summary>
        /// <param name="type">视频类型</param>
        /// <param name="subType">子类型</param>
        /// <param name="userId">用户id</param>
        /// <param name="fullPath">路径</param>
        /// <param name="boxNum">箱号</param>
        /// <param name="location">城市</param>
        /// <returns></returns>
        public async Task<int> AddVideoAsync(VideoType type,int subType, int userId, string fullPath, string boxNum, string location, double lng, double lat)
        {
            var db = base.CreateDb<Video>();

            var video_ = new Video();
            video_.Name = Path.GetFileNameWithoutExtension(fullPath);
            video_.Path = fullPath;
            video_.CreateTime = DateTime.Now;
            video_.VideoType = (int)type;
            video_.CreateUserId = userId;
            video_.BoxNum = string.IsNullOrEmpty(boxNum) ? null : boxNum;
            video_.Location = string.IsNullOrEmpty(location) ? null : location;
            video_.Lng = lng;
            video_.Lat = lat;
            video_.SubType = subType;

            db.Add(video_);

            var row = await db.SaveChangesAsync();
            return row;
        }

    }
}
