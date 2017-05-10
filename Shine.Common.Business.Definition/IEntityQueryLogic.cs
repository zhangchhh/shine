using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shine.Common.DataAccess.Entities;

namespace Shine.Common.Business.Definition
{
    /// <summary>查询操作的逻辑层接口</summary>
    /// <typeparam name="TResult">查询结果</typeparam>
    /// <typeparam name="TParam">查询参数</typeparam>
    public interface IEntityQueryLogic<TResult, in TParam> where TResult : IEntityBase where TParam : QueryParam
    {
        /// <summary>根据指定的查询参数执行查询，并生成分页信息</summary>
        /// <param name="queryParam">查询参数</param>
        /// <returns>查询结果</returns>
        PagedResults<TResult> Query(TParam queryParam);
    }
}
