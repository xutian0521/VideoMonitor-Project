using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoMonitor.Entities.ViewModel
{
    /// <summary>
    /// 分页泛型数据
    /// </summary>
    public class Paged<T> where T : class, new()
    {

        /// <summary>
        /// 每页行数
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int Total { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get
            {
                if (Total > 0)
                {
                    return Total % this.PageSize == 0 ? Total / this.PageSize : Total / this.PageSize + 1;
                }
                else
                {
                    return 0;
                }
            }
        }
        public List<T> Data { get; set; }
    }
}
