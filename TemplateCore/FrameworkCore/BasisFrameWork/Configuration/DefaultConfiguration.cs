using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace BasisFrameWork.Configuration
{
    /// <summary>
    /// 配置文件设置
    /// </summary>
   public  class DefaultConfiguration
    {
        /// <summary>
        /// 默认配置上下文
        /// </summary>
        public static IConfiguration Configuration { get; private set; }

        public static void SetDefaultConfiguration(IConfiguration configuration)
        {
            Configuration = configuration;
        }
    }
}
