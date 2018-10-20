using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using BasisFrameWork.Extension;
using Microsoft.Extensions.DependencyModel;

namespace BasisFrameWork.Assembly
{
    public static class AssemblyHelper
    {
        /// <summary>
        /// 获取程序集集合
        /// </summary>
        /// <param name="isContainService">是否包含Serviceable的程序集，默认为false</param>
        /// <param name="isContainPackage">是否包含类型为package的程序集,默认为false</param>
        /// <returns></returns>
        public static List<System.Reflection.Assembly> GetAssemblies(bool isContainService=false,bool isContainPackage=false)
        {
            return DependencyContext
                       .Default
                       .CompileLibraries
                       ?.WhereIf(!isContainService, it => !it.Serviceable)
                       ?.WhereIf(!isContainPackage,it=> it.Type != "package")
                       ?.Select(it => System.Reflection.Assembly.Load(new AssemblyName(it.Name)))
                       .Distinct()
                       .ToList()
                   ?? new List<System.Reflection.Assembly>();
        }

       



    }
}
