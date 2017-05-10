using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Shine.Common.DataAccess.Entities
{
    /// <summary>分页信息</summary>
    [DataContract]
    public class PagerInfo
    {
        /// <summary>获取或设置总记录数</summary>
        [DataMember]
        public int TotalRowCount { get; set; }

        /// <summary>获取或设置每页记录数</summary>
        [DataMember]
        public int PageSize { get; set; }

        /// <summary>获取或设置起始记录（从0开始）</summary>
        [DataMember]
        public int StartIndex { get; set; }

        /// <summary>获取当前页码（从1开始）</summary>
        public int PageIndex
        {
            get
            {
                return PagerInfo.ComputePageIndex(this.StartIndex + 1, this.PageSize);
            }
        }

        /// <summary>获取记录的总页数</summary>
        public int TotalPageCount
        {
            get
            {
                return PagerInfo.ComputePageIndex(this.TotalRowCount, this.PageSize);
            }
        }

        /// <summary>根据查询参数初始化分页信息</summary>
        /// <param name="queryParam"></param>
        public PagerInfo(QueryParam queryParam)
        {
            this.PageSize = queryParam.Rows;
            this.StartIndex = queryParam.StartIndex;
        }

        /// <summary>计算页码</summary>
        /// <param name="total"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        private static int ComputePageIndex(int total, int pageSize)
        {
            int result;
            int num = Math.DivRem(total, pageSize, out result);
            if (result > 0)
                ++num;
            return num;
        }
    }
}
