using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoMonitor.Entities.Infrastructure
{
    public class EFModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDbContextFactory>().To<DbContextFactory>();
        }
    }
}
