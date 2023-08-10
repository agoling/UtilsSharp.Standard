using System;
using System.Collections.Generic;
using System.Text;

namespace UtilsSharp.RabbitMq.Extension
{
    /// <summary>
    /// 接收参数
    /// </summary>
    public class BatchReceivedRequest
    {
        /// <summary>
        /// 业务类型名称
        /// </summary>
        public string RabbitMqBusinessName { set; get; }
        /// <summary>
        /// 每个消费者每次执行条数
        /// </summary>
        public int ConsumerHandleCount { set; get; }

        /// <summary>
        /// 是否一次channel
        /// </summary>
        public bool IsOnceChannel { set; get; }

        /// <summary>
        /// 如果队列无数据休眠时间：毫秒
        /// </summary>
        public int MillisecondsDelay { set; get; }

    }
}
