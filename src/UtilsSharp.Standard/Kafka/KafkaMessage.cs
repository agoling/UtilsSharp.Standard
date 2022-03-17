using System;
using System.Collections.Generic;
using System.Text;

namespace Kafka
{
    public class KafkaMessage
    {
        /// <summary>
        /// 主题
        /// </summary>
        public string Topic { get; set; }
        /// <summary>
        /// 分区，不指定分区即交给kafka指定分区
        /// </summary>
        public int? Partition { get; set; }
        /// <summary>
        /// 键值
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public object Message { get; set; }
    }
}
