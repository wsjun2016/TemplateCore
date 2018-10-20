using System;
using System.Collections.Generic;
using System.Text;

namespace BasisFrameWork.Extension.Redis
{
    public class RedisOption
    {
        /// <summary>
        /// 创建redis访问分区类，通过 KeyRule 对 key 进行分区。
        /// 按key分区规则，返回值格式：127.0.0.1:6379/13，默认方案(null)：取key哈希与节点数取模
        /// </summary>
        public Func<string, string> NodeRule { get; set; } = null;

        /// <summary>
        /// 连接字符串,如：
        /// 127.0.0.1[:6379],password=123456,defaultDatabase=13,poolsize=50,ssl=false,writeBuffer=10240,prefix=key前辍
        /// 默认为127.0.0.1:6379
        /// </summary>
        public string ConnectionString { get; set; } = "127.0.0.1:6379";
    }
}
