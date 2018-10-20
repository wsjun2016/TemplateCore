using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using BasisFrameWork.Assembly;
using BasisFrameWork.Dependency.Interfaces;
using BasisFrameWork.Extension;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BasisFrameWork.Dependency.Installers
{
    public static class DependencyInstaller
    {
        /// <summary>
        /// 注册所有自定义的服务
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomedServices(this IServiceCollection service)
        {
            var assemblies = AssemblyHelper.GetAssemblies();
            return  service.AddServicesByLifetime(DependencyLifetime.Singleton,assemblies)
                .AddServicesByLifetime(DependencyLifetime.Scoped, assemblies)
                .AddServicesByLifetime(DependencyLifetime.Transient, assemblies);
        }

        /// <summary>
        /// 根据生命周期注册服务
        /// </summary>
        /// <param name="service">服务容器</param>
        /// <param name="lifetime">生命周期类型</param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IServiceCollection AddServicesByLifetime(this IServiceCollection service, DependencyLifetime lifetime,IEnumerable<System.Reflection.Assembly> assemblies)
        {
            if(assemblies.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(assemblies));

            var baseType = lifetime == DependencyLifetime.Singleton
                ? typeof(ISingletonService)
                : lifetime == DependencyLifetime.Scoped
                    ? typeof(IScopedService)
                    : typeof(ITransientService);

            //选出当前服务接口的所有实现类型或接口
            var definedTypes = assemblies.SelectMany(it => it.DefinedTypes).Where(it=>baseType.IsAssignableFrom(it.AsType())).ToList();
            //选出派生的接口
            var interfaces = definedTypes.Where(it => it.IsInterface && it.AsType() != baseType);
            //选出派生的类型
            var impTypeInfos = definedTypes.Where(it => it.IsClass && !it.IsAbstract);

            foreach (var typeInfo in interfaces)
            {
                //选出当前接口派生的类型
                List<Type> impTypes = null;
                if (!typeInfo.IsGenericType)
                    impTypes = impTypeInfos.Where(it => typeInfo.IsAssignableFrom(it)).Select(it => it.AsType()).ToList();
                else
                    impTypes = impTypeInfos.Where(it => 
                        it.ImplementedInterfaces.Any(i =>
                            {
                                var info = i.GetTypeInfo();
                                return info.Namespace == typeInfo.Namespace && info.Name == typeInfo.Name;
                            })
                        ).Select(it => it.AsType()).ToList();

                if (impTypes.Count > 0)
                {
                    //注册服务
                    var interfaceType = typeInfo.AsType();
                    switch (lifetime)
                    {
                        case DependencyLifetime.Singleton:impTypes.ForEach(it => { service.TryAddSingleton(interfaceType, it);});break;
                        case DependencyLifetime.Scoped: impTypes.ForEach(it => { service.TryAddScoped(interfaceType, it); }); break;
                        default:impTypes.ForEach(it => { service.TryAddTransient(interfaceType, it); });break;
                    }
                }
            }

            return service;
        }

    }
}
