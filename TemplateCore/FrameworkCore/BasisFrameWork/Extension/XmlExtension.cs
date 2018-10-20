using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace BasisFrameWork.Extension
{
   public static partial class XmlExtension
    {
        /// <summary>
        /// 通过名称找到匹配的第一个xml节点
        /// </summary>
        /// <param name="document"></param>
        /// <param name="name">需要查找节点的名称</param>
        /// <param name="isIgnoreCase">在查找的时候，是否忽略名称的大小写 true:是  false:否</param>
        /// <returns></returns>
        public static XElement Find(this IEnumerable<XElement> document, string name, bool isIgnoreCase = true) {
            name = (name ?? "").Trim();
            if ((document?.Count() ?? 0) <= 0 || name.IsNullOrEmpty())
                return null;

            name = isIgnoreCase ? name.ToLower() : name;

            foreach (var item in document) {
                var itemName = isIgnoreCase ? item.Name.ToString().ToLower() : item.Name.ToString();
                if (itemName == name)
                    return item;
                if (item.HasElements) {
                    var result = item.Elements().Find(name, isIgnoreCase);
                    if (result != null)
                        return result;
                }

            }

            return null;
        }


    }
}
