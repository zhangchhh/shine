using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shine.Common.DataAccess.Implement
{
    /// <summary>所有数据访问的基类</summary>
    public abstract class BaseRepository
    {

        /// <summary>判断实体是否已经附加在数据环境</summary>
        /// <param name="activeContext">实体数据库</param>
        /// <param name="entity">业务实体</param>
        /// <returns>是否已经附加</returns>
        protected bool IsAttached(ObjectContext activeContext, object entity)
        {
            ObjectStateEntry entry;
            if (activeContext.ObjectStateManager.TryGetObjectStateEntry(entity, out entry) && entry != null)
                return entry.State != EntityState.Detached;
            return false;
        }

        /// <summary>通过实体集创建实体查询（不跟踪实体对象状态）</summary>
        /// <param name="objectSet">实体集</param>
        /// <returns>实体查询</returns>
        protected static ObjectQuery<T> CreateObjectQuery<T>(ObjectSet<T> objectSet) where T : class
        {
            ObjectQuery<T> query = objectSet.Context.CreateQuery<T>(objectSet.CommandText);
            query.MergeOption = MergeOption.NoTracking;
            return query;
        }
    }
}
