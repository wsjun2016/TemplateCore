using System;
using BasisFrameWork.Configuration;
using BasisFrameWork.Extension;

namespace BasisFrameWork.Utilities.Configuration
{
    public sealed partial class Cfg
    {
        /// <summary>
        /// 从默认配置文件中获取键名为key的value值
        /// </summary>
        /// <param name="key">键名</param>
        /// <returns></returns>
        public static string Get(string key)
        {
            return key.IsNullOrEmpty() ? "" : DefaultConfiguration.Configuration[key];
        }
    }
}
