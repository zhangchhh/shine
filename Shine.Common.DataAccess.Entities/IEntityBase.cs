using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shine.Common.DataAccess.Entities
{
    /// <summary>
    /// 实体类的基接口
    /// </summary>
    public interface IEntityBase
    {
        /// <summary>实体的唯一标识</summary>
        Guid Id { get; set; }
    }
}
