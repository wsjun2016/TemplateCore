using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using BasisFrameWork.Extension;

namespace BasisFrameWork.Assembly
{
   public static partial  class TypeHelper
   {
       private static List<Type> _types;
       private static readonly object _locker=new object();

       /// <summary>
       /// 从本地缓存中加载类型集合
       /// </summary>
       /// <returns></returns>
       private static List<Type> LoadOnCache()
       {
           if (_types == null)
           {
               lock (_locker)
               {
                   if (_types == null)
                       _types = GetTypesFromAssemblies();
               }
           }

           return _types;
       }

       /// <summary>
       /// 从程序集中获取类型集合
       /// </summary>
       /// <returns></returns>
       public static List<Type> GetTypesFromAssemblies()
        {
            var rc=new List<Type>();

            var assemblies = AssemblyHelper.GetAssemblies();
            if(assemblies.IsNullOrEmpty())
                return new List<Type>();

            assemblies.ForEach(it =>
            {
                Type[] tmpArray;
                try
                {
                    tmpArray = it.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    tmpArray = ex.Types;
                }
                
                if(!tmpArray.IsNullOrEmpty())
                    rc.AddRange(tmpArray.Where(type=>type!=null));
            });

            return rc;
        }

        /// <summary>
        /// 查找类型集合
        /// </summary>
        /// <param name="condition">需要查找的条件</param>
        /// <returns></returns>
       public static List<Type> Find(Func<Type,bool> condition)
       {
           return condition != null
               ? LoadOnCache()?.Where(condition).ToList() ?? new List<Type>()
               : throw new ArgumentNullException(nameof(condition));
       }

    }
}
