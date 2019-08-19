using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoMonitor.Business.Interface;

namespace VideoMonitor.Business.Infrastructure
{
    public class BusinessModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IAuthBusiness>().To<AuthBusiness>();
            Bind<IRoleBusiness>().To<RoleBusiness>();
            Bind<ISystemBusiness>().To<SystemBusiness>();
            Bind<IUserBusiness>().To<UserBusiness>();
            Bind<IVideoBusiness>().To<VideoBusiness>();
        }
    }
}
