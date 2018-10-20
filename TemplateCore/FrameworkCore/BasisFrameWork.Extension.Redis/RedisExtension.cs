using System;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

namespace BasisFrameWork.Extension.Redis
{
    public static partial class RedisExtension
    {
        public static IServiceCollection AddCSRedis(this IServiceCollection services)
        {
            return services.AddCSRedis(new RedisOption());
        }

        public static IServiceCollection AddCSRedis(this IServiceCollection services,RedisOption option)
        {
            if(option==null)
                option=new RedisOption();

            //初始化 RedisHelper
            var csredis = new CSRedis.CSRedisClient(option.NodeRule,option.ConnectionString);
            RedisHelper.Initialization(csredis);

            //注册mvc分布式缓存
            services.AddSingleton<IDistributedCache>(new Microsoft.Extensions.Caching.Redis.CSRedisCache(RedisHelper.Instance));

            return services;
        } 

    }
}
