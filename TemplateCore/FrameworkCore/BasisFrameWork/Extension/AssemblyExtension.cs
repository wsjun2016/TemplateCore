using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BasisFrameWork.Extension
{
    public static partial class AssemblyExtension
    {

        /// <summary>
        /// 通过 继承的接口名 查找 对应的 实体类型集合
        /// <remarks>
        /// 如： var list = ReflectionHelper.GetAssemblies()?.GetTypes(nameof(IEntity));
        /// </remarks>
        /// </summary>
        /// <param name="assemblies"></param>
        /// <param name="implementedInterfaceName">继承的接口名称</param>
        /// <returns></returns>
        public static IEnumerable<Type> GetTypes(this IEnumerable<System.Reflection.Assembly> assemblies, string implementedInterfaceName) {
            List<Type> rc = new List<Type>();
            implementedInterfaceName = implementedInterfaceName?.Trim();
            if (!string.IsNullOrWhiteSpace(implementedInterfaceName) && assemblies?.Count() > 0) {
                foreach (var assembly in assemblies) {
                    var types = assembly?.DefinedTypes?.Where(it => it.ImplementedInterfaces.Any(item => item.Name == implementedInterfaceName)).Select(it => it.UnderlyingSystemType);
                    if (types?.Count() > 0)
                        rc.AddRange(types.ToList());
                }
            }

            return rc;
        }

        /// <summary>
        /// 通过 拥有的特性名 和 在该特性中的公共属性名和属性值 去 查找 对应的 实体类型集合
        /// <remarks>
        /// 如： Type type = ReflectionHelper.GetAssemblies()?.GetTypes(nameof(IEntity))?.GetTypes(typeof(SugarTable), "TableName", tableName)?.First();
        /// </remarks>
        /// </summary>
        /// <param name="types"></param>
        /// <param name="attributeType"></param>
        /// <param name="pubPropertyNameInAttribute"></param>
        /// <param name="pubPropertyValueInAttribute"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetTypes(this IEnumerable<Type> types, Type attributeType, string pubPropertyNameInAttribute = "", object pubPropertyValueInAttribute = null) {
            List<Type> rc = new List<Type>();

            pubPropertyNameInAttribute = pubPropertyNameInAttribute?.Trim();

            if (attributeType != null && types?.Count() > 0) {
                foreach (var type in types) {
                    var list = type.GetCustomAttributes(attributeType, false);
                    if (list?.Length > 0) {
                        if (string.IsNullOrWhiteSpace(pubPropertyNameInAttribute))
                            rc.Add(type);
                        else {
                            foreach (var item in list) {
                                var pro = item.GetType().GetProperty(pubPropertyNameInAttribute);
                                if (pro != null) {
                                    var value = pro.GetValue(item, null);
                                    if ((value == null && pubPropertyValueInAttribute == null) || (value != null && value.Equals(pubPropertyValueInAttribute)))
                                        rc.Add(type);
                                }
                            }

                        }
                    }
                }
            }

            return rc;
        }

        /// <summary>
        /// 获取DescriptionAttribute描述信息
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static string GetDescription(this MemberInfo property) {
            if (property == null)
                return null;
            var desc = property.GetCustomAttributes(typeof(DescriptionAttribute), false).Cast<DescriptionAttribute>().FirstOrDefault();
            return desc == null ? "" : desc.Description;
        }


    }
}
