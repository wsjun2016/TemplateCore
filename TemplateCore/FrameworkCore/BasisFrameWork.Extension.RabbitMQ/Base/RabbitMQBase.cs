using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;

namespace BasisFrameWork.Extension.RabbitMQ.Base
{
    public abstract class RabbitMQBase {
        /// <summary>
        /// 配置选项，下方到子类中实现
        /// </summary>
        protected RabbitMQOption Option { get; set; }

        public ConnectionFactory ConnectionFactory => GetConnectionFactory();

        public IConnection GetConnection() {
            return ConnectionFactory.CreateConnection();
        }

        /// <summary>
        /// 初始化一个连接工厂
        /// </summary>
        /// <returns></returns>
        protected ConnectionFactory GetConnectionFactory() {
            return new ConnectionFactory
            {
                HostName = Option?.HostName,
                UserName = Option?.UserName,
                Password = Option?.Password,
                VirtualHost = Option?.VirtualHost,
            };
        }
    }
}
