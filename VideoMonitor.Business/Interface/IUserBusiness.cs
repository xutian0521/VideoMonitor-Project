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
    public interface IUserBusiness
    {
        #region 递归用户层 操作
        /// <summary>
        /// 递归获取用户层(子父级)
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="userLayers"></param>
        /// <returns></returns>
        Task<UserlayersVM> RecursionUserlayersAsync(List<int> ids, UserlayersVM userLayers);

        /// <summary>
        /// 递归用户层(子父级) 获取userId 的集合
        /// </summary>
        /// <param name="userLayers"></param>
        /// <param name="allUserId"></param>
        /// <returns></returns>
        List<int> GetAllChildUserId(UserlayersVM userLayers, List<int> allUserId);
        /// <summary>
        /// 根据自己id 获取自己所在下的子用户 的id集合 包含自己的id
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        Task<List<int>> GetAllUserIdAsync(int userId);
        Task<UserlayersVM> GetUserlayersAsync(string userName);
        #endregion
        /// <summary>
        /// 获取未删除的所以用户
        /// </summary>
        /// <returns></returns>
        Task<List<User>> GetAllUserListAsync();
        /// <summary>
        /// 根据id获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<User> GetUserByIdAsync(int id);
        /// <summary>
        /// 根据id 获取所有子用户和自己 的user集合
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<User>> GetUserListAsync(int userId);
        /// <summary>
        /// 根据id 获取用户的viewmodel
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<UserInfoViewModel> GetUserViewModelAsync(int userId);
        /// <summary>
        /// 根据id 获取分页的所有子用户和自己 的user集合
        /// </summary>
        /// <param name="parm"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Paged<UserInfoViewModel>> GetUserPageListAsync(Pagination parm, int userId, UserType userType, string keyWords);



        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<LoginResultViewModel> RegisterAsync(User model);

        /// <summary>
        /// 新增或 修改用户信息的viewmodel
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响的行数</returns>
        Task<int> AddOrModifyUserAsync(UserInfoViewModel model);
        /// <summary>
        /// 新增或 修改user
        /// </summary>
        /// <param name="user_"></param>
        /// <returns></returns>
        Task<int> AddOrModifyUserAsync(User user_);

        /// <summary>
        /// 根据id 删除user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteByIdAsync(int id);

        /// <summary>
        /// 检查用户信息是否完整
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> CheckUserInfoIsIntactAsync(int userId);
    }
}
