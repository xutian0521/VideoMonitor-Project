using Ninject;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoMonitor.Entities.Infrastructure;

namespace VideoMonitor.Business.Infrastructure
{
    public class DBRepository
    {
        IKernel kernal = new StandardKernel(new EFModule());
        public DbBase<T> CreateDb<T>()where T:class,new()
        {

            //Samurai s = new Samurai(kernal.Get<IWeapon>()); // 构造函数注入
            var s = kernal.Get<DbBase<T>>();
            return s;
        }
    }
}
