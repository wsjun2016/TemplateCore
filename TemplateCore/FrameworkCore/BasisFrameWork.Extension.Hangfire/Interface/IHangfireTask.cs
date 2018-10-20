using System;
using System.Collections.Generic;
using System.Text;
using BasisFrameWork.Dependency.Interfaces;

namespace BasisFrameWork.Extension.Hangfire.Interface
{
    /// <summary>
    ///  Hangfire任务接口，任何继承该接口的都会加入到Hangfire任务
    /// </summary>
    public interface IHangfireTask : ITransientService {
        /// <summary>
        /// 运行hangfire任务
        /// <remarks>
        ///     Run方法里面可以加入定时任务的代码，如下所示：
        ///     //定时任务，从每个小时的0分开始，每5分钟执行一次
        ///     RecurringJob.AddOrUpdate(()=>RunTask(),"*/5 * * * *");
        /// </remarks>
        /// </summary>
        void Run();
    }
}
