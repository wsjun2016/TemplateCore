using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BasisFrameWork.Extension
{
    public static partial class EnumExtension {
        public static string GetDescription(this Type type, dynamic value) {
            return type.GetFields(BindingFlags.Static | BindingFlags.Public)
                .FirstOrDefault(item => item.Name == value.ToString()
                                        || Enum.Parse(type, item.Name).Equals(value)
                                        || ((int)Enum.Parse(type, item.Name)).Equals(value)
                )
                .GetDescription();
        }
    }
}
