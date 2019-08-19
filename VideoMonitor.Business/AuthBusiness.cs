using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoMonitor.Business.Infrastructure;
using VideoMonitor.Common;
using VideoMonitor.Entities.Models;
using VideoMonitor.Entities.ViewModel;
using System.Data.Entity;
using System.Web;
using VideoMonitor.Entities.Enums;
using VideoMonitor.Business.Interface;

namespace VideoMonitor.Business
{
    public class AuthBusiness:  BusinessBase, IAuthBusiness
    {
        //登陆
        public async Task<LoginResultViewModel> UserLoginAsync(string userName, string passWord)
        {
            var AuthResult = new LoginResultViewModel();

            AuthResult.IsSuccess = false;
            var db = base.CreateDb<User>();
            try
            {
                var user = await db.Query().Where(t => t.isDeleted == false && t.UserName == userName)
    .Include(t => t.UserRoles.Select(p => p.Role)).FirstOrDefaultAsync();
                if (user == null)
                {
                    AuthResult.MessageResult = "不存在该用户！";
                    return AuthResult;
                }

                if (user.PassWord != passWord)
                {
                    AuthResult.MessageResult = "密码错误！";
                }
                else
                {
                    AuthResult.IsSuccess = true;
                    AuthResult.UserId = user.Id;//登录成功，返回该用户的id

                    var roles = user.UserRoles.FirstOrDefault();
                    if (roles != null)
                    {

                        var userType = this.GetUserType(roles.Role);//获取用户类型枚举
                        AuthResult.UserType = userType;
                    }

                }
            }
            catch (Exception)
            {
                
                throw;
            }


            return AuthResult;
        }


        //检查用户名是否存在
        public async Task<bool> CheckUserNameIsExistsAsync(string userName)
        {
            var IsExist = true;
            
            var db = base.CreateDb<User>();

            IsExist = await db.Query().AnyAsync(t => t.UserName == userName);
            return IsExist;
        }

        /// <summary>
        /// 检查是否登录
        /// </summary>
        /// <returns></returns>
        public bool CheckIsLogin()
        {
            var isLogin = false;

            var Context = HttpContext.Current;
            var cookie = Context.Request.Cookies;

            if (cookie["UserToken"] != null)
            {
                var value_ = cookie["UserToken"].Value;
                if (!string.IsNullOrEmpty(value_))
                {
                    isLogin = true;
                }
            }

            return isLogin;
        }

        /// <summary>
        /// 根据用户id 获取用户类型
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<UserType> GetUserTypeAsync(int userId)
        {
            UserType userType = UserType.None;
            var db = base.CreateDb<User>();
            var user = await db.Query().Where(t => t.Id == userId).Include(t=>t.UserRoles.Select(p=>p.Role)).FirstOrDefaultAsync();
            var roles = user.UserRoles.FirstOrDefault();
            if (roles != null)
            {
                var role_ = roles.Role;
                userType= this.GetUserType(role_);
            }
            return userType;
        }
        /// <summary>
        /// 根据用户id 获取用户类型 --非异步
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserType GetUserType(int userId)
        {
            UserType userType = UserType.None;
            var db = base.CreateDb<User>();
            var user = db.Query().Where(t => t.Id == userId).Include(t => t.UserRoles.Select(p => p.Role)).FirstOrDefault();
            var roles = user.UserRoles.FirstOrDefault();
            if (roles != null)
            {
                var role_ = roles.Role;
                userType = this.GetUserType(role_);
            }
            return userType;
        }

        /// <summary>
        /// 根据角色名称获取角色类型枚举
        /// </summary>
        /// <param name="role_"></param>
        /// <returns></returns>
        public UserType GetUserType(Role role_)
        {
            UserType userType = UserType.None;
            if (role_ != null)
            {
                switch (role_.RoleName)
                {
                    default:
                        userType = UserType.None;
                        break;
                    case "管理员":
                        userType = UserType.Admin;
                        break;
                    case "业务员":
                        userType = UserType.Salesman;
                        break;
                    case "客户":
                        userType = UserType.Customer;
                        break;

                }
            }
            return userType;
        }

        /// <summary>
        /// 传入平台识别码，和授权码，并核对平台识别码是否合法，并且授权码是否可用，则写入授权码到服务器根目录文件。
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AuthorizePlat(AuthorizeVM model)
        {
            var isAuthorize = false;

            PlatformAuthorizeHelper helper = new PlatformAuthorizeHelper();
            var MachineCode = helper.GetMachineCode();
            if (MachineCode == model.MachineCode)
            {
                isAuthorize = helper.CheckPlatformIsAuthorize(model.AuthorizeCode);//如果授权码可用

                if (isAuthorize)
                {
                    helper.WriteAuthorizeToConfig(model.AuthorizeCode);
                }
                
            }
            return isAuthorize;
        }
        public bool CheckPlatformIsAuthorize()
        {
            PlatformAuthorizeHelper helper = new PlatformAuthorizeHelper();
            bool isAuthorize = helper.CheckPlatformIsAuthorize();//如果授权码可用

            return isAuthorize;
        }
    }
}
