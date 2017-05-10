using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shine.Common.DataAccess.Entities;

namespace Shine.Common.Business.Definition
{
    /// <summary>获取所有对象的业务逻辑接口</summary>
    /// <typeparam name="T"></typeparam>
    public interface IEntityGetAllLogic<T> where T : IEntityBase
    {
        /// <summary>获取所有实体对象</summary>
        /// <returns>实体对象清单</returns>
        IList<T> GetAll();
    }
}
