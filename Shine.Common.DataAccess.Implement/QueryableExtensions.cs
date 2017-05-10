using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Shine.Common.DataAccess.Entities;

namespace Shine.Common.DataAccess.Implement
{
    /// <summary>IQueryable扩展</summary>
    public static class QueryableExtensions
    {
        /// <summary>将查询语句转换为查询结果</summary>
        /// <typeparam name="T">查询结果的类型</typeparam>
        /// <param name="source">查询语句</param>
        /// <param name="queryParam">查询参数</param>
        /// <returns></returns>
        public static PagedResults<T> ToPagedResults<T>(this IQueryable<T> source, QueryParam queryParam)
        {
            if (queryParam.StartIndex < 0)
                throw new InvalidOperationException("起始记录数不能小于0");
            if (queryParam.Rows <= 0)
                throw new InvalidOperationException("每页记录数不能小于0");
            ObjectQuery<T> objectQuery = source as ObjectQuery<T>;
            if (objectQuery != null)
                objectQuery.MergeOption = MergeOption.NoTracking;
            int num = 0;
            TransactionOptions transactionOptions = new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };
            using (TransactionScope transactionScope = new TransactionScope((TransactionScopeOption)num, transactionOptions))
            {
                PagerInfo pagerInfo = new PagerInfo(queryParam)
                {
                    TotalRowCount = Queryable.Count<T>(source)
                };
                source = string.IsNullOrWhiteSpace(queryParam.Sidx) ? source : source.OrderBy<T>(queryParam.OrderString);
                List<T> list = Queryable.Take<T>(Queryable.Skip<T>(source, queryParam.StartIndex), queryParam.Rows).ToList<T>();
                transactionScope.Complete();
                return new PagedResults<T>()
                {
                    PagerInfo = pagerInfo,
                    Data = (IList<T>)list
                };
            }
        }
    }
}
