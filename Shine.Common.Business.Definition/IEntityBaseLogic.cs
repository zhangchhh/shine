using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shine.Common.DataAccess.Entities;

namespace Shine.Common.Business.Definition
{
    /// <summary>CURD基础操作的业务逻辑接口</summary>
    /// <typeparam name="T">实体类型</typeparam>
    public interface IEntityBaseLogic<T> where T : IEntityBase
    {
        /// <summary>创建实体对象</summary>
        /// <param name="entity">实体对象</param>
        /// <param name="status">操作状态</param>
        void Create(T entity, out OperateStatus status);

        /// <summary>修改实体对象</summary>
        /// <param name="entity">实体对象</param>
        /// <param name="status">操作状态</param>
        void Update(T entity, out OperateStatus status);

        /// <summary>删除实体对象</summary>
        /// <param name="id">实体对象ID</param>
        /// <param name="status">操作状态</param>
        void Delete(Guid id, out OperateStatus status);

        /// <summary>获取实体对象</summary>
        /// <param name="id">实体对象ID</param>
        /// <returns>实体对象</returns>
        T GetById(Guid id);

        /// <summary>通过指定Id清单获取实体对象清单</summary>
        /// <param name="values">实体Id清单</param>
        /// <returns>实体对象清单</returns>
        IList<T> GetByValues(IList<Guid> values);
    }
}
