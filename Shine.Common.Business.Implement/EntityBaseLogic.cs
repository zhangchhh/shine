using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shine.Common.Business.Definition;
using Shine.Common.DataAccess.Definition;
using Shine.Common.DataAccess.Entities;

namespace Shine.Common.Business.Implement
{
    /// <summary>实体业务逻辑基类</summary>
    /// <typeparam name="T">业务实体类</typeparam>
    public abstract class EntityBaseLogic<T> : IEntityBaseLogic<T> where T : class, IEntityBase
    {
        private readonly IRepository<T> repository;

        /// <summary>构造函数</summary>
        /// <param name="repository">注入数据访问接口</param>
        protected EntityBaseLogic(IRepository<T> repository)
        {
            if (repository == null)
                throw new ArgumentNullException("repository", "Repository cannot be null");
            this.repository = repository;
        }

        /// <summary>创建实体对象</summary>
        /// <param name="entity">实体对象</param>
        /// <param name="status">操作状态</param>
        public virtual void Create(T entity, out OperateStatus status)
        {
            if ((object)entity == null)
                throw new ArgumentNullException("entity");
            if (entity.Id == Guid.Empty)
                entity.Id = Guid.NewGuid();
            this.repository.Insert(entity);
            status = new OperateStatus()
            {
                ResultSign = ResultSign.Successful,
                MessageKey = "保存成功"
            };
        }

        /// <summary>更新实体对象</summary>
        /// <param name="entity">实体对象</param>
        /// <param name="status">操作状态</param>
        public virtual void Update(T entity, out OperateStatus status)
        {
            if ((object)entity == null)
                throw new ArgumentNullException("entity");
            if (entity.Id == Guid.Empty)
                throw new ArgumentException("实体的Id为空。");
            status = new OperateStatus();
            T byId = this.GetById(entity.Id);
            if ((object)byId == null)
            {
                status.ResultSign = ResultSign.Error;
                status.MessageKey = "未找到实体id";
            }
            else
            {
                this.repository.Update(byId, entity);
                status.ResultSign = ResultSign.Successful;
                status.MessageKey = "保存失败";
            }
        }

        /// <summary>删除实体对象</summary>
        /// <param name="id">实体对象Id</param>
        /// <param name="status">操作状态</param>
        public virtual void Delete(Guid id, out OperateStatus status)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("id为空。");
            status = new OperateStatus();
            T byId = this.GetById(id);
            if ((object)byId == null)
            {
                status.ResultSign = ResultSign.Error;
                status.MessageKey = "100014";
            }
            else
            {
                this.repository.Delete(byId);
                status.ResultSign = ResultSign.Successful;
                status.MessageKey = "删除成功";
            }
        }

        /// <summary>通过ID获取实体对象</summary>
        /// <param name="id">实体对象Id</param>
        /// <returns>实体对象</returns>
        public virtual T GetById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("id为空");
            return this.repository.GetById(id);
        }

        /// <summary>通过指定Id清单获取实体对象清单</summary>
        /// <param name="values">实体Id清单</param>
        /// <returns>实体对象清单</returns>
        public virtual IList<T> GetByValues(IList<Guid> values)
        {
            return this.repository.GetByValues(values);
        }
    }
}
