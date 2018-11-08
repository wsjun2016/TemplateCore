using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TemplateCore.Attributes {
    /// <summary>
    /// 不使用TrimModelBinder，作用在类或属性上
    /// </summary>
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Property,AllowMultiple = false)]
    public class NoTrimAttribute :Attribute{
    }
}
