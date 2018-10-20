using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using log4net;
using log4net.Config;
using log4net.Repository;

namespace BasisFrameWork.Logger.log4net
{
    public static partial class Log4NetExtension {
        //配置文件，我将之放在站点根目录的Config文件夹中
        private static string Log4netConfigureName => "log4net.config";

        //日志仓储的名称
        private static string RepositoryName => "Log4NetLogRepository";

        //全局使用一个日志仓储。
        //注意：不允许定义多个同名的日志仓储。
        public static ILoggerRepository Repository => LogManager.CreateRepository(RepositoryName);

        static Log4NetExtension() {
            //配置已经被实例化的日志仓储
            XmlConfigurator.Configure(Repository, new FileInfo(Log4netConfigureName));
        }

        /// <summary>
        /// 获取ILog实例
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ILog GetLog(this Type type) {
            if (type == null)
                throw new ArgumentNullException();
            //只要有了日志仓库，就可以得到一个ILog的实例。因此，在这之前必须实例化一个日志仓储
            return LogManager.GetLogger(RepositoryName, type);
        }

        /// <summary>
        /// 获取ILog实例
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static ILog GetLog<TEntity>(this TEntity entity) where TEntity : class {
            //只要有了日志仓库，就可以得到一个ILog的实例。因此，在这之前必须实例化一个日志仓储
            return LogManager.GetLogger(RepositoryName, typeof(TEntity));
        }
    }
}
