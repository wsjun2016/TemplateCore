using System;
using System.Collections.Generic;
using System.Text;

namespace BasisFrameWork.Configuration
{
    /// <summary>
    /// 服务定位器
    /// </summary>
    public class ServiceLocator {
        public static IServiceProvider Services { get; private set; }

        public static void SetServices(IServiceProvider services) {
            Services = services;
        }
    }
}
