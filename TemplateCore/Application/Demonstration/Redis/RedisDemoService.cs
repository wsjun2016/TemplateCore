using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Demonstration.Redis
{
    public class RedisDemoService: IRedisDemoService
    {
        private readonly string Key = "REDISDEMOKEY";

        //采用异步设置
        public async Task<bool> Set(string value)
        {
            return await RedisHelper.SetAsync(Key, value);
        }

        //采用异步获取
        public async Task<string> Get()
        {
            return await RedisHelper.GetAsync(Key);
        }
    }
}
