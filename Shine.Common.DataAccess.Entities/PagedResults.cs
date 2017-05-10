using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Shine.Common.DataAccess.Entities
{
    /// <summary>分页查询结果</summary>
    [KnownType(typeof(PagerInfo))]
    [DataContract(IsReference = true)]
    public class PagedResults<T>
    {
        /// <summary>分页信息</summary>
        [DataMember]
        public PagerInfo PagerInfo { get; set; }

        /// <summary>数据</summary>
        [DataMember]
        public IList<T> Data { get; set; }

        /// <summary>默认构造函数</summary>
        public PagedResults()
        {
        }

        /// <summary>构造一个无数据清单的查询结果</summary>
        public PagedResults(QueryParam param)
        {
            this.PagerInfo = new PagerInfo(param);
            this.Data = (IList<T>)new List<T>();
        }

        /// <summary>根据传入的转换方法将分页结果转换为指定类型的分页结果</summary>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public PagedResults<TResult> ConvertTo<TResult>(Func<T, TResult> converter)
        {
            return new PagedResults<TResult>()
            {
                PagerInfo = this.PagerInfo,
                Data = (IList<TResult>)this.Data.Select<T, TResult>(converter).ToArray<TResult>()
            };
        }

        /// <summary>将传入的分页结果强制转换为指定类型的分页结果</summary>
        /// <returns></returns>
        public static PagedResults<T> ConvertFrom(object pagedResults)
        {
            Type type = pagedResults.GetType();
            object obj = type.GetProperty("PagerInfo").GetValue(pagedResults, (object[])null);
            IEnumerable<T> source = (IEnumerable<T>)typeof(Enumerable).GetMethod("Cast", BindingFlags.Static | BindingFlags.Public).MakeGenericMethod(typeof(T)).Invoke((object)null, new object[1]
            {
        type.GetProperty("Data").GetValue(pagedResults, (object[]) null)
            });
            return new PagedResults<T>()
            {
                PagerInfo = (PagerInfo)obj,
                Data = (IList<T>)source.ToArray<T>()
            };
        }
    }
}
