using System;
using System.Collections.Generic;
using System.Text;

namespace BasisFrameWork.Dependency
{
    /// <summary>
    /// 依赖注入服务的生命周期
    /// </summary>
    public enum DependencyLifetime
    {
        /// <summary>
        /// 全局
        /// </summary>
        Singleton,
        /// <summary>
        /// 作用域
        /// </summary>
        Scoped,
        /// <summary>
        /// 瞬态
        /// </summary>
        Transient
    }
}
