using System;
using System.Collections.Generic;
using System.Text;
using BasisFrameWork.Configuration;
using BasisFrameWork.Extension.RabbitMQ.Base;
using Microsoft.Extensions.DependencyInjection;

namespace BasisFrameWork.Extension.RabbitMQ {
    public static partial class RabbitMQExtension {
        public static IServiceCollection AddRabbitMQ(this IServiceCollection services,Action<RabbitMQOption> action)
        {
            services.Configure<RabbitMQOption>(action);

            return services;
        }

        public static IServiceCollection AddRabbitMQ(this IServiceCollection services) {
            services.Configure<RabbitMQOption>(DefaultConfiguration.Configuration.GetSection("RabbitMq"));

            return services;
        }
    }
}
