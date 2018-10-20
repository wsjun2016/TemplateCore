using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace BasisFrameWork.Extension {
    /// <summary>
    /// 对linq的扩展
    /// </summary>
    public static partial class LinqExtension {

        /// <summary>
        /// 条件筛选
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="source">源集合数据</param>
        /// <param name="isWhere">true:执行condition条件筛选  false:不执行任何筛选</param>
        /// <param name="condition">当isWhere为true时，执行的筛选条件</param>
        /// <returns></returns>
        public static IEnumerable<TEntity> WhereIf<TEntity>(this IEnumerable<TEntity> source,bool isWhere, Func<TEntity, bool> condition)
        {
            if(condition==null)
                throw new ArgumentNullException(nameof(condition));

            return isWhere ? source?.Where(condition) : source;
        }

        /// <summary>
        /// 判断当前集合是否为空
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<TEntity>(this IEnumerable<TEntity> source)
        {
            return (source?.Count()??0) <= 0;
        }

        /// <summary>
        /// 判断当前数组是否为空
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<TEntity>(this TEntity[] source) {
            return (source?.Length ?? 0) <= 0;
        }

        /// <summary>
        /// 对集合进行分页处理
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="source"></param>
        /// <param name="skip">跳过多少条数据，默认为0</param>
        /// <param name="take">获取多少条数据，默认为10</param>
        /// <returns></returns>
        public static IEnumerable<TEntity> PageBy<TEntity>(this IEnumerable<TEntity> source,int skip=0,int take=10)
        {
            return source.IsNullOrEmpty() ? source : source.Skip(skip).Take(take);
        }
    }
}
