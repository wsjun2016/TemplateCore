using System;
using System.Collections.Generic;
using System.Text;

namespace BasisFrameWork.Extension.RabbitMQ.Base
{
    public class RabbitMQOption
    {
        /// <summary>
        /// 主机地址，如：localhost
        /// </summary>
        public string HostName { get; set; }
        /// <summary>
        /// 登录的用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 登录的密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 虚拟机名称
        /// </summary>
        public string VirtualHost { get; set; }
    }
}
