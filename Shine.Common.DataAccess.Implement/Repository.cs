using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Shine.Common.DataAccess.Definition;
using Shine.Common.DataAccess.Entities;

namespace Shine.Common.DataAccess.Implement
{
    /// <summary>数据访问层操作实现基类</summary>
    /// <typeparam name="T">实体类</typeparam>
    public abstract class Repository<T>: BaseRepository, IRepository<T> where T : class, IEntityBase
    {
        /// <summary>实体数据库</summary>
        protected abstract ObjectContext ActiveContext { get; }

        /// <summary>实体对象集合(查询)</summary>
        protected virtual System.Data.Objects.ObjectQuery<T> ObjectQuery
        {
            get
            {
                return BaseRepository.CreateObjectQuery<T>(this.ObjectSet);
            }
        }

        /// <summary>实体对象集合(操作)</summary>
        protected abstract System.Data.Objects.ObjectSet<T> ObjectSet { get; }

        /// <summary>创建到数据环境</summary>
        /// <param name="item">业务实体</param>
        protected virtual void AddObject(T item)
        {
            this.ObjectSet.AddObject(item);
        }

        /// <summary>判断实体是否已经附加在数据环境</summary>
        /// <param name="item">业务实体</param>
        /// <returns>是否已经附加</returns>
        protected bool IsAttached(T item)
        {
            return this.IsAttached(this.ActiveContext, (object)item);
        }

        /// <summary>附加到数据环境</summary>
        /// <param name="item">业务实体</param>
        protected virtual void Attach(T item)
        {
            if (this.IsAttached(item))
                return;
            this.ObjectSet.Attach(item);
        }

        /// <summary>应用当前数据变更</summary>
        /// <param name="item">业务实体</param>
        protected virtual void ApplyCurrentValues(T item)
        {
            this.ObjectSet.ApplyCurrentValues(item);
        }

        /// <summary>实体接受所有变更,重置到UnChanged状态,用于缓存更新前</summary>
        /// <param name="item">业务实体</param>
        protected virtual void AcceptChanges(T item)
        {
        }

        /// <summary>删除实体对象</summary>
        /// <param name="item">实体对象</param>
        protected virtual void DeleteObject(T item)
        {
            if (!this.IsAttached(item))
                this.ObjectSet.DeleteObject(this.ObjectSet.Single<T>((Expression<Func<T, bool>>)(o => o.Id == item.Id)));
            else
                this.ObjectSet.DeleteObject(item);
        }

        /// <summary>根据Id从数据库获取对象</summary>
        /// <param name="id">实体Id</param>
        /// <returns>实体对象</returns>
        protected virtual T GetByIdFromDataBase(Guid id)
        {
            return this.ObjectQuery.SingleOrDefault<T>((Expression<Func<T, bool>>)(o => o.Id == id));
        }

        /// <summary>从数据库获取所有对象</summary>
        /// <returns>实体对象清单</returns>
        protected virtual IList<T> GetAllFromDataBase()
        {
            return (IList<T>)this.ObjectQuery.ToList<T>();
        }

        /// <summary>根据Id获取对象</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T GetById(Guid id)
        {
            T byIdFromDataBase = this.GetByIdFromDataBase(id);
            return byIdFromDataBase;
        }

        /// <summary>插入对象</summary>
        /// <param name="entity"></param>
        public virtual void Insert(T entity)
        {
            this.AddObject(entity);
            this.ActiveContext.SaveChanges();
        }

        /// <summary>删除对象</summary>
        /// <param name="entity"></param>
        public virtual void Delete(T entity)
        {
            if (!this.IsAttached(entity))
                this.DeleteObject(this.ObjectSet.Single<T>((Expression<Func<T, bool>>)(o => o.Id == entity.Id)));
            else
                this.DeleteObject(entity);
            this.ActiveContext.SaveChanges();
        }

        /// <summary>更新对象</summary>
        /// <param name="original">原始数据</param>
        /// <param name="current">当前数据</param>
        public virtual void Update(T original, T current)
        {
            if (!this.IsAttached(original))
            {
                original = this.ObjectSet.Single<T>((Expression<Func<T, bool>>)(o => o.Id == current.Id));
                this.Attach(original);
            }
            this.ApplyCurrentValues(current);
            this.ActiveContext.SaveChanges();
            this.AcceptChanges(original);
        }
        /// <summary>将当前实体对象的变更更新到数据库</summary>
        /// <param name="current">实体对象当前值</param>
        public void Update(T current)
        {
            if (!this.IsAttached(current))
                this.Attach(this.ObjectSet.Single<T>((Expression<Func<T, bool>>)(o => o.Id == current.Id)));
            this.ApplyCurrentValues(current);
            this.ActiveContext.SaveChanges();
            this.AcceptChanges(current);
        }

        /// <summary>获取所有对象</summary>
        /// <exception cref="T:System.NotImplementedException"></exception>
        /// <returns>实体对象清单</returns>
        public virtual IList<T> GetAll()
        {
            IList<T> allFromDataBase = this.GetAllFromDataBase();
            return allFromDataBase;
        }

        /// <summary>通过指定Id清单获取实体对象清单</summary>
        /// <param name="values">实体Id清单</param>
        /// <returns>实体对象清单</returns>
        public virtual IList<T> GetByValues(IList<Guid> values)
        {
            return (IList<T>)this.ObjectQuery.Where<T>((Expression<Func<T, bool>>)(e => values.Contains(e.Id))).ToList<T>();
        }

        /// <summary>检查实体是否存在依赖项</summary>
        /// <param name="propertyName">检查的属性</param>
        /// <param name="id">属性的值</param>
        /// <returns>是否存在依赖项</returns>
        public bool HasDependence(string propertyName, Guid id)
        {
            ObjectParameter objectParameter = new ObjectParameter(propertyName, (object)id);
            return this.ObjectQuery.Where(string.Format("it.{0}=@{1}", (object)propertyName, (object)propertyName), objectParameter).Any<T>();
        }
    }
}
