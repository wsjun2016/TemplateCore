using System;
using System.Collections.Generic;
using System.Text;

namespace BasisFrameWork.Extension
{
    public static partial class ByteExtension
    {
        /// <summary>
        /// 将二进制缓存转换为BASE64字符串
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static string ToBase64(this byte[] buffer)
        {
            if (buffer.IsNullOrEmpty())
                return "";
            return Convert.ToBase64String(buffer);
        }
    }
}
