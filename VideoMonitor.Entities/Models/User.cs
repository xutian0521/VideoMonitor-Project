using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoMonitor.Entities.Infrastructure;
using VideoMonitor.Entities.ViewModel;

namespace VideoMonitor.Entities.Models
{
    public class User: EntityBase
    {
        public User()
        {
            this.UserRoles = new List<UserRole>();
            this.Videos = new List<Video>();
        }
        public int Id { get; set; }

        [DisplayName("联系人")]
        [Required(ErrorMessage = "请输入真实姓名")]
        public string RealName { get; set; }

        [Required(ErrorMessage ="请输入用户名")]
        [DisplayName("用户名")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "请输入密码")]
        [DisplayName("密码")]
        public string PassWord { get; set; }//密码长度 6~16位


        [DisplayName("联系方式")]
        [Required(ErrorMessage = "请输入联系方式")]
        public string Phone { get; set; }

        [DisplayName("公司名称")]
        public string CompanyName { get; set; }
        public int ParentId { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<Video> Videos { get; set; }

        //ext
        public virtual UserInfoViewModel ToViewModel()
        {
            var viewModel = new UserInfoViewModel();

            viewModel.Id = this.Id;
            viewModel.CompanyName = this.CompanyName;
            viewModel.ParentId = this.ParentId;
            viewModel.PassWord = this.PassWord;
            viewModel.Phone = this.Phone;
            viewModel.RealName = this.RealName;
            viewModel.Remark = this.Remark;
            viewModel.UserName = this.UserName;
            viewModel.RoleId = this.UserRoles.FirstOrDefault().RoleId;
            viewModel.RoleName = this.UserRoles.FirstOrDefault().Role.RoleName;

            return viewModel;
        }

    }
}
