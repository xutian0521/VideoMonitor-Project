using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoMonitor.Entities.Infrastructure
{
    public interface IDbContextFactory
    {
        DbContext GetContext();
    }
}
