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
    public interface IAuthBusiness
    {
        //登陆
        Task<LoginResultViewModel> UserLoginAsync(string userName, string passWord);


        //检查用户名是否存在
        Task<bool> CheckUserNameIsExistsAsync(string userName);

        /// <summary>
        /// 检查是否登录
        /// </summary>
        /// <returns></returns>
        bool CheckIsLogin();

        /// <summary>
        /// 根据用户id 获取用户类型
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<UserType> GetUserTypeAsync(int userId);
        /// <summary>
        /// 根据用户id 获取用户类型 --非异步
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        UserType GetUserType(int userId);

        /// <summary>
        /// 根据角色名称获取角色类型枚举
        /// </summary>
        /// <param name="role_"></param>
        /// <returns></returns>
        UserType GetUserType(Role role_);

        /// <summary>
        /// 传入平台识别码，和授权码，并核对平台识别码是否合法，并且授权码是否可用，则写入授权码到服务器根目录文件。
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool AuthorizePlat(AuthorizeVM model);
    }
}
