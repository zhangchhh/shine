using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shine.Common.DataAccess.Entities;

namespace Shine.Common.DataAccess.Definition
{
    /// <summary>查询的操作类</summary>
    /// <typeparam name="TResult">查询结果的类型</typeparam>
    /// <typeparam name="TParam">查询参数的类型</typeparam>
    public interface IQueryRepository<TResult, in TParam> where TResult : IEntityBase where TParam : QueryParam
    {
        /// <summary>根据指定的查询参数执行查询，并生成分页信息</summary>
        /// <param name="queryParam">查询参数</param>
        /// <returns>查询结果</returns>
        PagedResults<TResult> Query(TParam queryParam);
    }
}
