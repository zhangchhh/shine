using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Shine.Common.DataAccess.Entities
{
    /// <summary>
    /// 查询参数的基类
    /// </summary>
    [DataContract(IsReference = true)]
    public abstract class QueryParam
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        protected QueryParam()
        {
            Rows = 10;
            Page = 1;
            Sord = "asc";
        }

        /// <summary>
        /// 获取起始记录
        /// <para>从0开始</para>
        /// </summary>
        public int StartIndex
        {
            get
            {
                return (Page - 1) * Rows;
            }
        }

        /// <summary>
        /// 起始页码
        /// <para>从1开始</para>
        /// </summary>
        [DataMember]
        public int Page { get; set; }

        /// <summary>
        /// 每页记录数，默认10
        /// </summary>
        [DataMember]
        public int Rows { get; set; }

        /// <summary>
        /// 排序方式，默认升序（asc）
        /// </summary>
        [DataMember]
        public string Sord { get; set; }

        /// <summary>
        /// 用来排序的字段
        /// </summary>
        [DataMember]
        public string Sidx { get; set; }

        /// <summary>
        /// 获取排序字符串
        /// </summary>
        public string OrderString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Sidx))
                    Sidx = "Id";
                return string.Format("{0} {1}", Sidx, Sord);
            }
        }

    }
}
