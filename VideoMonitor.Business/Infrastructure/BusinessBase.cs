using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoMonitor.Entities.Infrastructure;

namespace VideoMonitor.Business.Infrastructure
{
    public class BusinessBase
    {
        protected DbBase<T> CreateDb<T>() where T : class, new()
        {
            var repository = new DBRepository();
            return repository.CreateDb<T>();
        }
    }
}
