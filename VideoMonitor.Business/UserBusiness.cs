using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoMonitor.Business.Infrastructure;
using VideoMonitor.Entities.Models;
using System.Data.Entity;
using VideoMonitor.Entities.ViewModel;
using System.Web;
using VideoMonitor.Common;
using VideoMonitor.Business.Interface;
using VideoMonitor.Entities.Enums;

namespace VideoMonitor.Business
{
    public class UserBusiness: BusinessBase, IUserBusiness
    {
        #region 递归用户层 操作
        /// <summary>
        /// 递归获取用户层(子父级)
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="userLayers"></param>
        /// <returns></returns>
        public async Task<UserlayersVM> RecursionUserlayersAsync(List<int> ids, UserlayersVM userLayers)
        {
            var db = base.CreateDb<User>();

            if (await db.Query().AnyAsync(t => ids.Contains(t.ParentId)))
            {

                List<User> users = await db.GetListByAsync(t => ids.Contains(t.ParentId));
                userLayers.ChildAccounts = users.Select(t => new UserlayersVM { Self = t }).ToList();

                foreach (var item in userLayers.ChildAccounts)
                {
                    await RecursionUserlayersAsync(new List<int>() { item.Self.Id }, item);
                }

            }

            return userLayers;
        }

        /// <summary>
        /// 递归用户层(子父级) 获取userId 的集合
        /// </summary>
        /// <param name="userLayers"></param>
        /// <param name="allUserId"></param>
        /// <returns></returns>
        public List<int> GetAllChildUserId(UserlayersVM userLayers, List<int> allUserId)
        {
            if (userLayers.Self != null)
            {
                allUserId.Add(userLayers.Self.Id);
            }


            if (userLayers.ChildAccounts != null)
            {

                if (userLayers.ChildAccounts.Count > 0)
                {
                    foreach (var item in userLayers.ChildAccounts)
                    {

                        this.GetAllChildUserId(item, allUserId);
                    }

                }
            }
            return allUserId;
        }
        /// <summary>
        /// 根据自己id 获取自己所在下的子用户 的id集合 包含自己的id
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        public async Task<List<int>> GetAllUserIdAsync(int userId)
        {

            var allUserlayers = await this.RecursionUserlayersAsync(new List<int>() { userId }, new UserlayersVM());

            List<int> allUserId = new List<int>();

            this.GetAllChildUserId(allUserlayers, allUserId);
            allUserId.Add(userId);//包括自己的id


            return allUserId;
        }
        public async Task<UserlayersVM> GetUserlayersAsync(string userName)
        {
            var db = base.CreateDb<User>();

            var result = new UserlayersVM();
            var user = await db.GetFirstAsync(t => t.UserName == userName);

            if (user != null)
            {
                var userLayers = new UserlayersVM();
                userLayers.Self = user;

                result = await this.RecursionUserlayersAsync(new List<int>() { user.Id }, userLayers);
            }

            return result;
        }
        #endregion

        /// <summary>
        /// 获取未删除的所以用户
        /// </summary>
        /// <returns></returns>
        public async Task<List<User>> GetAllUserListAsync()
        {
            var db = base.CreateDb<User>();
            var list = await db.Where(t => t.isDeleted == false).ToListAsync();
            return list;
        }
        /// <summary>
        /// 根据id获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<User> GetUserByIdAsync(int id)
        {
            var db = base.CreateDb<User>();
            var user = await db.GetFirstAsync(t => t.isDeleted == false && t.Id == id);

            return user;
        }
        /// <summary>
        /// 根据id 获取所有子用户和自己 的user集合
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<User>> GetUserListAsync(int userId)
        {
            var db = base.CreateDb<User>();

            var allIdList = await this.GetAllUserIdAsync(userId);
            var list = await db.Query().Where(t => allIdList.Contains(t.Id)).ToListAsync();

            return list;
        }
        /// <summary>
        /// 根据id 获取用户的viewmodel
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<UserInfoViewModel> GetUserViewModelAsync(int userId)
        {
            var vm = new UserInfoViewModel();

            var db = base.CreateDb<User>();
            var user = await db.Query().Where(t => t.Id == userId).Include(t => t.UserRoles.Select(r => r.Role))
                .FirstOrDefaultAsync();

            return user.ToViewModel();
        }
        /// <summary>
        /// 根据id 获取分页的所有子用户和自己 的user集合
        /// </summary>
        /// <param name="parm"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Paged<UserInfoViewModel>> GetUserPageListAsync(Pagination parm, int userId, UserType userType, string keyWords)
        {
            var db = base.CreateDb<User>();
            IQueryable<User> Query = db.Query();

            var allIdList = await this.GetAllUserIdAsync(userId);

            if (userType == UserType.Admin)//如果是管理员 则可以看到其他管理员的账号信息
            {
                Query = Query.Where(t => allIdList.Contains(t.Id) || t.ParentId == 0);
            }
            else//不是管理员 只能看到自己和 自己的子用户
            {
                Query = Query.Where(t => allIdList.Contains(t.Id));
            }
            //关键字搜索
            if (!string.IsNullOrEmpty(keyWords))
            {
                Query = Query.Where(t => t.UserName.Contains(keyWords) || t.RealName.Contains(keyWords) || t.Phone.Contains(keyWords) || t.CompanyName.Contains(keyWords));
            }
            var paged = await Query.Include(t => t.UserRoles.Select(r => r.Role)).ToPagedListAsync(parm, t => t.Id);


            var viewModelList = new List<UserInfoViewModel>();
            var pagedViewModel = new Paged<UserInfoViewModel>()
            {
                PageIndex = paged.PageIndex,
                PageSize = paged.PageSize,
                Total = paged.Total,
                Data = viewModelList
            };

            var userList = await db.Where(t => t.isDeleted == false).Include(t=>t.UserRoles).ToListAsync();//查出所有用户
            foreach (var item in paged.Data)
            {
                var _viewModel = item.ToViewModel();
                //如果集合中该用户的父级ID对应的用户是业务员，那么给viewmodel所属业务员字段赋值
                var _user = userList.Where(t => t.Id == _viewModel.ParentId && t.UserRoles.FirstOrDefault().RoleId == 2).FirstOrDefault();
                if (_user != null)
                {
                    _viewModel.SalesmanName = _user.RealName;
                }

                viewModelList.Add(_viewModel);
            }

            return pagedViewModel;
        }



        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<LoginResultViewModel> RegisterAsync(User model)
        {
            var AuthResult = new LoginResultViewModel();

            AuthResult.IsSuccess = false;

            var db = base.CreateDb<User>();
            db.Add(model);

            var row = await db.SaveChangesAsync();
            if (row > 0)
            {
                AuthResult.IsSuccess = true;
            }

            return AuthResult;
        }

        /// <summary>
        /// 新增或 修改用户信息的viewmodel
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响的行数</returns>
        public async Task<int> AddOrModifyUserAsync(UserInfoViewModel model)
        {
            var row = 0;

            var userDb = base.CreateDb<User>();
            var userRoleDb = base.CreateDb<UserRole>();

            var user_ = await userDb.Query().Where(t => t.Id == model.Id).Include(t=>t.UserRoles).FirstOrDefaultAsync();

            if (model.Id > 0)//如果id 大于0 说明是修改
            {
                user_.CompanyName = model.CompanyName;

                if (model.PassWord.Count() != 32)
                {
                    user_.PassWord =MD5Helper.MD5Encrypt( model.PassWord);
                }

                user_.Phone = !string.IsNullOrEmpty(model.Phone) ? model.Phone : "";
                user_.RealName = !string.IsNullOrEmpty(model.RealName) ? model.RealName : "";
                user_.Remark = model.Remark;
                user_.UserName = model.UserName;


                userDb.Modifies(user_);
            }
            else//否则是新增
            {
                //新增时 判断当前新增用户是否存在(用户名是否相同)

                if (await userDb.Query().Where(t => t.UserName == model.UserName).FirstOrDefaultAsync() ==null)//是否存在该用户
                {
                    user_ = new User()
                    {
                        Id = model.Id,
                        CompanyName = model.CompanyName,
                        ParentId = model.RoleId != (int)UserType.Admin ? model.ParentId : 0,//新增才 插入ParentId
                        PassWord = MD5Helper.MD5Encrypt(model.PassWord),
                        Phone = !string.IsNullOrEmpty(model.Phone) ? model.Phone : "",
                        RealName = !string.IsNullOrEmpty(model.RealName) ? model.RealName : "",
                        Remark = model.Remark,
                        UserName = model.UserName,
                        CreateTime = DateTime.Now,
                        UserRoles = new List<UserRole>() { new UserRole() { RoleId = model.RoleId, CreateTime = DateTime.Now } }

                    };

                    userDb.Add(user_);
                }
                else
                {
                    return -1;
                }

            }
            try
            {
                row = await userDb.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                
                throw ex;
            }

            return row;
        }
        /// <summary>
        /// 新增或 修改user
        /// </summary>
        /// <param name="user_"></param>
        /// <returns></returns>
        public async Task<int> AddOrModifyUserAsync(User user_)
        {
            var row = 0;

            var userDb = base.CreateDb<User>();
            if (user_.Id > 0)
            {
                if (user_.PassWord.Count() != 32)
                {
                    user_.PassWord = MD5Helper.MD5Encrypt(user_.PassWord);
                }
                userDb.Attch(user_);
                userDb.Modify(user_, t => t.PassWord);
                userDb.Modify(user_, t => t.RealName);
                userDb.Modify(user_, t => t.Phone);
                userDb.Modify(user_, t => t.CompanyName);
                userDb.Modify(user_, t => t.RealName);
            }
            else
            { 
                userDb.Add(user_);
            }
            try
            {
                row = await userDb.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return row;
        }

        /// <summary>
        /// 根据id 删除user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteByIdAsync(int id)
        {
            var db = base.CreateDb<User>();

            var user = new User();
            user.Id = id;

            db.Delete(user);

            var row = await db.SaveChangesAsync();

            return row;
        }

        /// <summary>
        /// 检查用户信息是否完整
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> CheckUserInfoIsIntactAsync(int userId)
        {
            var IsIntact =false;

            var db = base.CreateDb<User>();
            var user = await db.GetFirstAsync(t => t.Id == userId);

            if (!string.IsNullOrEmpty(user.RealName) && !string.IsNullOrEmpty(user.Phone) 
                &&!string.IsNullOrEmpty(user.CompanyName))
            {
                IsIntact = true;
            }

            return IsIntact;
        }

    }
}
