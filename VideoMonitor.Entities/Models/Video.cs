using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoMonitor.Entities.Infrastructure;
using VideoMonitor.Entities.ViewModel;

namespace VideoMonitor.Entities.Models
{
    public class Video : EntityBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }

        public int Sort { get; set; }
        public int CreateUserId { get; set; }
        public int VideoType { get; set; }
        public string BoxNum { get; set; }

        public string Location { get; set; }
        public double Lng { get; set; }
        public double Lat { get; set; }
        public int SubType { get; set; }

        public virtual User User { get; set; }
        public virtual DataDictionary DataDictionary { get;set; }



        //ext
        public virtual VideoViewModel ToViewModel()
        {
            var viewModel = new VideoViewModel()
            {
                CreateTime = this.CreateTime,
                CreateUserId = this.CreateUserId,
                Id = this.Id,
                isDeleted = this.isDeleted,
                Name = this.Name,
                Path = this.Path,
                Remark = this.Remark,
                Sort = this.Sort,
                BoxNum = this.BoxNum,
                UserName = this.User.UserName,
                RoleName = this.User.UserRoles.First().Role.RoleName,
                CompanyName = this.User.CompanyName,
                Phone = this.User.Phone,
                SubType = this.DataDictionary.TypeValue,
                Location = this.Location,
                ParentId=this.User.ParentId,
            };
            return viewModel;

        }
    }
}
