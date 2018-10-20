﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using AutoMapper;

namespace BasisFrameWork.Extension.AutoMapper
{
    public static class AutoMapperConfigurationExtensions {
        /// <summary>
        /// 创建映射配置属性的扩展
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="type"></param>
        public static void CreateAutoAttributeMaps(this IMapperConfigurationExpression configuration, Type type) {
            foreach (var autoMapAttribute in type.GetTypeInfo().GetCustomAttributes<AutoMapAttributeBase>()) {
                autoMapAttribute.CreateMap(configuration, type);
            }
        }
    }
}
