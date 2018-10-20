using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BasisFrameWork.Logger.log4net;
using log4net;

namespace Application.Services.Imp
{
    public class DemoService: IDemoService
    {
        private readonly ILog _log;
        private readonly string Key = "SYSDICKEYNAME";

        public DemoService()
        {
            _log = this.GetLog();
        }

        public async ValueTask<bool> Set(string value)
        {
            _log.Info(value);
          return await RedisHelper.SetAsync(Key, value);
        }

        public async Task<string> Get()
        {
           return await RedisHelper.GetAsync(Key);
        }
    }
}
