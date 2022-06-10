using System;
using System.Collections.Generic;
using System.Text;

namespace Kafka
{
    public class KafkaSubscriber
    {
        /// <summary>
        /// 主题
        /// </summary>
        public string Topic { get; set; }
        /// <summary>
        /// 分区
        /// </summary>
        public int? Partition { get; set; }
    }
}
