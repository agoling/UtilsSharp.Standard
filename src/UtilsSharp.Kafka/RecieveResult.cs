using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Confluent.Kafka;

namespace UtilsSharp.Kafka
{
    public class RecieveResult
    {
        CancellationTokenSource cancellationTokenSource;

        internal RecieveResult(ConsumeResult<string, object> consumeResult,
            CancellationTokenSource cancellationTokenSource)
        {
            this.Topic = consumeResult.Topic;
            this.Message = consumeResult.Message.Value?.ToString();
            this.Offset = consumeResult.Offset.Value;
            this.Partition = consumeResult.Partition.Value;
            this.Key = consumeResult.Message.Key;

            this.cancellationTokenSource = cancellationTokenSource;
        }

        /// <summary>
        /// Kafka消息所属的主题
        /// </summary>
        public string Topic { get; private set; }

        /// <summary>
        /// 键值
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// 我们需要处理的消息具体的内容
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Kafka数据读取的当前位置
        /// </summary>
        public long Offset { get; private set; }

        /// <summary>
        /// 消息所在的物理分区
        /// </summary>
        public int Partition { get; private set; }

        /// <summary>
        /// 提交
        /// </summary>
        public void Commit()
        {
            if (cancellationTokenSource == null || cancellationTokenSource.IsCancellationRequested) return;

            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
            cancellationTokenSource = null;
        }
    }
}
