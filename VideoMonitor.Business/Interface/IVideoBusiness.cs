using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoMonitor.Entities.Enums;
using VideoMonitor.Entities.Models;
using VideoMonitor.Entities.ViewModel;

namespace VideoMonitor.Business.Interface
{
    public interface IVideoBusiness
    {
        /// <summary>
        /// 根据id获取 第一个video
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Video> GetVideoByIdAsync(int id);

        /// <summary>
        /// 根据 userId获取该用户下和子用户下的所有视频集合
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<Video>> GetVideosByUserIdAsync(int userId);

        /// <summary>
        /// 根据 userId获取该用户下和子用户下的所有视频 分页的集合
        /// </summary>
        /// <param name="parm">分页参数</param>
        /// <param name="userId">用户id</param>
        /// <param name="videoType">视频类型</param>
        /// <param name="keyWords">关键字</param>
        /// <returns>分页的视频viewmodel集合</returns>
        Task<Paged<VideoViewModel>> GetPagedVideosByUserIdAsync(Pagination parm, int userId, int videoType, string keyWords);
        /// <summary>
        /// 根据id 删除视频
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteByIdAsync(int id);

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
        Task<int> AddVideoAsync(VideoType type, int subType, int userId, string fullPath, string boxNum, string location, double lng, double lat);

    }
}

