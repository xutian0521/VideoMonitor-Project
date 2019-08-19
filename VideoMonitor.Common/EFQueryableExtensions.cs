using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoMonitor.Entities.ViewModel;
using System.Data.Entity;
using System.Linq.Expressions;

namespace VideoMonitor.Common
{
    /// <summary>
    /// EF Queryable扩展类
    /// </summary>
    public static class EFQueryableExtensions
    {
        /// <summary>
        /// 为EF扩展的 分页方法
        /// </summary>
        /// <typeparam name="T">分页集合的泛型 参数</typeparam>
        /// <typeparam name="TKey">排序类型的泛型 参数</typeparam>
        /// <param name="q"></param>
        /// <param name="parms">分页参数</param>
        /// <param name="orderBy">排序的lambda</param>
        /// <returns>分页的集合</returns>
        public static async Task<Paged<T>> ToPagedListAsync<T,TKey>(this IQueryable<T> q, Pagination parms, Expression<Func<T, TKey>> orderBy) where T : class, new()
        {
            var paged = new Paged<T>();
            
            paged.PageIndex = parms.PageIndex;
            paged.PageSize = parms.PageSize;
            paged.Total =await q.CountAsync();

            paged.Data = await q.OrderBy(orderBy).Skip(parms.PageSize*(parms.PageIndex-1)).Take(parms.PageSize).ToListAsync();
            return paged;
        }
    }
}
