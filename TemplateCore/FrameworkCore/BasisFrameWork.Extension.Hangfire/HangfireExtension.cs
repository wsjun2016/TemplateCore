using System;
using System.Collections.Generic;
using System.Linq;
using BasisFrameWork.Configuration;
using BasisFrameWork.Extension.Hangfire.Interface;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BasisFrameWork.Extension.Hangfire
{
    public static class HangfireExtension
    {
        public static void AddHangfire(this IServiceCollection services) {
            //获取application.json配置文件中ConnectionStrings节点下的HangfireConnectionString字符串配置
            //如：
            //"ConnectionStrings": {
            //"HangfireConnectionString": "server=47.96.69.51;uid=sa;pwd=ty2018,;database=Cdpc_Hangfire",
            //}
            var connectionString = DefaultConfiguration
                .Configuration
                .GetConnectionString("HangfireConnectionString");
            services.AddHangfire(x => x.UseSqlServerStorage(connectionString));
        }

        public static void UseHangfire(this IApplicationBuilder app, DashboardOptions options = null) {
            app.UseHangfireServer();
            app.UseHangfireDashboard("/hangfire", options);
            InitHangfireTasks();
        }

        /// <summary>
        /// 初始化Hangfire任务
        /// </summary>
        private static void InitHangfireTasks()
        {
            //找到继承了IHangfireTask接口的实例
            IList<IHangfireTask> list = ServiceLocator.Services.GetServices<IHangfireTask>()?.ToList();
            if (list?.Count > 0) {
                foreach (var item in list) {
                    item.Run();
                }
            }
        }
    }
}
