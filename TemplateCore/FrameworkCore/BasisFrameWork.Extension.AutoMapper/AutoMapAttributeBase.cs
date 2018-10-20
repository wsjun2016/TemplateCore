using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;

namespace BasisFrameWork.Extension.AutoMapper
{
    public abstract class AutoMapAttributeBase : System.Attribute {
        public Type[] TargetTypes { get; private set; }

        protected AutoMapAttributeBase(params Type[] targetTypes) {
            TargetTypes = targetTypes;
        }

        /// <summary>
        /// 创建映射
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="type"></param>
        public abstract void CreateMap(IMapperConfigurationExpression configuration, Type type);
    }
}
