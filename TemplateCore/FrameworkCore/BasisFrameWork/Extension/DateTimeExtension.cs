using System;
using System.Collections.Generic;
using System.Text;

namespace BasisFrameWork.Extension
{
   public static partial class DateTimeExtension {
        /// <summary>
        /// 获取当前时间的Timestamp时间戳
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static long ToTimestamp(this DateTime datetime) {
            return (long)(datetime - Convert.ToDateTime("1970-01-01T00:00:00")).TotalSeconds;
        }
    }
}
