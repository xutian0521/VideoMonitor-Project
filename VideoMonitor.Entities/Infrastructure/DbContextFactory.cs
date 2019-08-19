using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using VideoMonitor.Model;

namespace VideoMonitor.Entities.Infrastructure
{
    public class DbContextFactory: IDbContextFactory
    {
        /// <summary>
        /// 为了保持线程 EF上下文唯一，当前线程只允许创建一个EF上下文。
        /// </summary>
        /// <returns></returns>
        public DbContext GetContext()
        {
            //从当前线程中 获取 EF上下文对象
            var dbContext = CallContext.GetData("VideoMonitorDataBase_DbContext") as VideoMonitorDataBase;
            if (dbContext==null)
            {
                dbContext = new VideoMonitorDataBase();
                dbContext.Configuration.LazyLoadingEnabled = false;//延迟加载
                dbContext.Configuration.AutoDetectChangesEnabled = false;//自动追踪
                dbContext.Configuration.ValidateOnSaveEnabled = false;//自动验证
                dbContext.Configuration.ProxyCreationEnabled = false;//代理类
                //将新创建的 ef上下文对象 存入线程
                CallContext.SetData("VideoMonitorDataBase_DbContext", dbContext);
            }
            return dbContext;
        }
    }
}
