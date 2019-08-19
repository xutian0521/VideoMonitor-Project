using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VideoMonitor.Business;
using VideoMonitor.Business.Interface;
using VideoMonitor.Common.Attributes;
using VideoMonitor.Entities.Enums;
using VideoMonitor.Entities.Models;
using VideoMonitor.Entities.ViewModel;
using VideoMonitor.Web.Controllers.Base;

namespace VideoMonitor.Web.Controllers
{
    public class UserController : ControllerBasic
    {
        [ValidateAuthorize]
        [SkipLogonValidated]
        public ActionResult Login()
        {
            return View();
        }
        // GET: User 用户登录方法
        [SkipLogonValidated]
        public async Task<ActionResult> LoginAsync(User model)
        {
            var authBll = base.CreateBusiness<IAuthBusiness>();

            var result = await authBll.UserLoginAsync(model.UserName, model.PassWord);
            if (result.IsSuccess)
            {
                var cookies = new List<HttpCookie>()
                {
                    new HttpCookie("UserToken", model.UserName),//把用户名存入cookie
                    new HttpCookie("userId", result.UserId.ToString()),//把用户id存入cookie
                    new HttpCookie("userType", result.UserType.ToString()),//把用户类型存入cookie
                };

                foreach (var item in cookies)
                {
                    item.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(item);
                }
            }

            return Json(result);
        }
        //用户退出
        public ActionResult Logout()
        {
            Request.Cookies.Clear();
            var cookieUserName_ = new HttpCookie("UserToken", "")//把用户名存入cookie
            {
                Expires = DateTime.Now.AddMinutes(-1)
            };
            Response.Cookies.Add(cookieUserName_);
            return Redirect("/user/login");
        }
        public async Task<ActionResult> Register(User model)
        {
            var bll = base.CreateBusiness<IUserBusiness>();
            var result = await bll.RegisterAsync(model);
            return View();
        }

        //用户列表页
        public ActionResult Index()
        {
            return View();
        }

        //根据客服端 发过来cookie 的userId，获取user的viewmodel的 json数据
        public async Task<ActionResult> GetUserViewModelJsonAsync()
        {
            int userId = base.UserId;

            var bll = base.CreateBusiness<IUserBusiness>();
            var user = await bll.GetUserViewModelAsync(userId);

            return Json(user, JsonRequestBehavior.AllowGet);
        }

        //获取分页的用户 列表有 json 数据
        public async Task<ActionResult> GetUserPagedListJsonAsync(Pagination parm,string keyWords)
        {
            int userId = base.UserId;
            var userType=(UserType)Enum.Parse(typeof(UserType),Request.Cookies["userType"].Value);
            var bll = base.CreateBusiness<IUserBusiness>();
            var data = await bll.GetUserPageListAsync(parm, userId, userType, keyWords);

            return Json(data,JsonRequestBehavior.AllowGet);
        }

        //根据id 获取用户编辑的viewmodel json 数据
        public async Task<ActionResult> GetModifyJsonAsync(int userId)
        {
            var bll = base.CreateBusiness<IUserBusiness>();
            var data = await bll.GetUserViewModelAsync(userId);

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //添加用户
        public ActionResult Add()
        {
            return View("Edit");
        }

        //修改用户
        public ActionResult Modify()
        {
            return View("Edit");
        }

        //编辑用户界面视图
        public ActionResult Edit()
        {
            return View();
        }

        //提交新增，修改 用户
        public async Task<ActionResult> EditAsync(UserInfoViewModel model)
        {
            var bll = base.CreateBusiness<IUserBusiness>();

            var row = await bll.AddOrModifyUserAsync(model);
            if (row > 0)
            {
                return base.Success("操作成功。");
            }
            else
            {
                if (row == -1)
                {
                    return base.Error("该用户已存在！");
                }
                else
                {
                    return base.Error("操作失败。");
                }
                
            }
            
        }

        //删除用户
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var bll = base.CreateBusiness<IUserBusiness>();

            try
            {
                var row = await bll.DeleteByIdAsync(id);
            }
            catch (Exception ex)
            {
                return base.Error("删除失败！---" + ex.Message);
                throw ex;
            }

            return Success("删除成功！");
        }

        //个人信息   --mobile
        public async Task<ActionResult> Info()
        {
            var userId = this.UserId;

            var bll = base.CreateBusiness<IUserBusiness>();
            var model = await bll.GetUserByIdAsync(UserId);

            return View(model);
        }

        //提交修改跟人信息   --mobile
        public async Task<ActionResult> InfoEditAsync(User model)
        {
            var bll = base.CreateBusiness<IUserBusiness>();

            var row = await bll.AddOrModifyUserAsync(model);
            var redirectUrl= Request.Cookies["RedirectUrl"].Value;

            return Redirect(redirectUrl);
        }

        //检查用户信息是否完整
        public async Task<ActionResult> CheckUserInfoIsIntactAsync()
        {
            var userId = this.UserId;

            var bll = base.CreateBusiness<IUserBusiness>();
            var IsIntact = await bll.CheckUserInfoIsIntactAsync(userId);

            return Json(IsIntact, JsonRequestBehavior.AllowGet);

        }
    }
}