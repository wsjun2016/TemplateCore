using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Demonstration.Redis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TemplateCore.Controllers.Redis
{
   // [Produces("application/json")]
    [Route("api/RedisDemo")]
    public class RedisDemoController : Controller
    {
        private readonly IRedisDemoService _redisDemoService;

        public RedisDemoController(IRedisDemoService redisDemoService)
        {
            _redisDemoService = redisDemoService;
        }

        /// <summary>
        /// 采用异步方式设置
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet("v1/set/{value}")]
        public async Task<bool> Set(string value)
        {
            return await _redisDemoService.Set(value);
        }

        /// <summary>
        /// 采用异步方式获取值
        /// </summary>
        /// <returns></returns>
        [HttpGet("v1/get")]
        public async Task<string> Get() {
            return await _redisDemoService.Get();
        }
    }
}