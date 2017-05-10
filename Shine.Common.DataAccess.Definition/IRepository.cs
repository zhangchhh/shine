using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shine.Common.DataAccess.Entities;

namespace Shine.Common.DataAccess.Definition
{
    /// <summary>数据访问层操作基接口</summary>
    /// <typeparam name="T">实体类</typeparam>
    public interface IRepository<T> where T : IEntityBase
    {
        /// <summary>根据Id获取实体对象</summary>
        /// <param name="id">实体标识</param>
        /// <returns></returns>
        T GetById(Guid id);

        /// <summary>插入实体对象到数据库</summary>
        /// <param name="entity">实体对象</param>
        void Insert(T entity);

        /// <summary>从数据库删除实体对象</summary>
        /// <param name="entity">实体对象</param>
        void Delete(T entity);

        /// <summary>将当前实体对象更新到数据库</summary>
        /// <param name="original">实体对象原始值</param>
        /// <param name="current">实体对象当前值</param>
        void Update(T original, T current);

        /// <summary>将已经附加的当前实体对象的变更更新到数据库</summary>
        /// <remarks>实体对象在变更前已经附加在数据库上下文中</remarks>
        /// <param name="current">实体对象当前值</param>
        void Update(T current);

        /// <summary>获取所有实体对象</summary>
        /// <returns>实体对象清单</returns>
        IList<T> GetAll();

        /// <summary>通过指定Id清单获取实体对象清单</summary>
        /// <param name="values">实体Id清单</param>
        /// <returns>实体对象清单</returns>
        IList<T> GetByValues(IList<Guid> values);
    }
}
