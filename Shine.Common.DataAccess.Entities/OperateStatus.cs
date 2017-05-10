using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Shine.Common.DataAccess.Entities
{
    /// <summary>调用服务或业务逻辑的操作状态</summary>
    [DataContract]
    public class OperateStatus
    {
        /// <summary>获取一个值，表示返回标记是否成功</summary>
        public bool IsSuccessful
        {
            get
            {
                return this.ResultSign == ResultSign.Successful;
            }
        }

        /// <summary>获取一个值，表示返回标记是否不成功</summary>
        public bool IsNotSuccessful
        {
            get
            {
                return this.ResultSign != ResultSign.Successful;
            }
        }

        /// <summary>返回标记</summary>
        [DataMember]
        public ResultSign ResultSign { get; set; }

        /// <summary>消息字符串key</summary>
        [DataMember]
        public string MessageKey { get; set; }

        /// <summary>消息的参数</summary>
        [DataMember]
        public string[] FormatParams { get; set; }

        /// <summary>构造函数</summary>
        public OperateStatus()
        {
            this.ResultSign = ResultSign.Successful;
            this.MessageKey = string.Empty;
        }

        /// <summary>从操作状态构造</summary>
        public OperateStatus(OperateStatus status)
        {
            this.ResultSign = status.ResultSign;
            this.MessageKey = status.MessageKey;
            this.FormatParams = status.FormatParams;
        }

        /// <summary>从操作状态复制</summary>
        /// <param name="status">其他操作状态</param>
        public void CopyFromStatus(OperateStatus status)
        {
            this.ResultSign = status.ResultSign;
            this.MessageKey = status.MessageKey;
            this.FormatParams = status.FormatParams;
        }
    }

    /// <summary>调用服务或业务逻辑的返回标记</summary>
    [DataContract]
    public enum ResultSign
    {
        [EnumMember] Successful,
        [EnumMember] Warning,
        [EnumMember] Error
    }
}
