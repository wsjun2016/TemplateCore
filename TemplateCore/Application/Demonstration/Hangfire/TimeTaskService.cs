using System;
using System.Collections.Generic;
using System.Text;
using BasisFrameWork.Extension.Hangfire.Interface;
using BasisFrameWork.Logger.log4net;
using Hangfire;
using log4net;

namespace Application.Demonstration.Hangfire
{
    public class TimeTaskService : IHangfireTask {
        private readonly ILog _log;
        public TimeTaskService() {
            _log = this.GetLog();
        }
        public void Run() {
            RecurringJob.AddOrUpdate(() => RunTask(), "*/5 * * * *");
        }

        public void RunTask() {
            _log.Info($"当前时间:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
        }

    }
}
