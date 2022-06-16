using System;
using System.Collections.Generic;
using System.Text;
using Confluent.Kafka;

namespace UtilsSharp.Kafka
{
    public class DeliveryResult
    {
        internal DeliveryResult(DeliveryResult<string, object> deliveryResult)
        {
            this.Topic = deliveryResult.Topic;
            this.Partition = deliveryResult.Partition.Value;
            this.Offset = deliveryResult.Offset.Value;
            switch (deliveryResult.Status)
            {
                case PersistenceStatus.NotPersisted: this.Status = DeliveryResultStatus.NotPersisted; break;
                case PersistenceStatus.Persisted: this.Status = DeliveryResultStatus.Persisted; break;
                case PersistenceStatus.PossiblyPersisted: this.Status = DeliveryResultStatus.PossiblyPersisted; break;
            }
            this.Key = deliveryResult.Key;
            this.Message = deliveryResult.Value;

            if (deliveryResult is DeliveryReport<string, object>)
            {
                var dr = deliveryResult as DeliveryReport<string, object>;
                this.IsError = dr.Error.IsError;
                this.Reason = dr.Error.Reason;
            }
        }

        /// <summary>
        /// 是否异常
        /// </summary>
        public bool IsError { get; private set; }
        /// <summary>
        /// 异常原因
        /// </summary>
        public string Reason { get; private set; }
        /// <summary>
        /// 主题
        /// </summary>
        public string Topic { get; private set; }
        /// <summary>
        /// 分区
        /// </summary>
        public int Partition { get; private set; }
        /// <summary>
        /// 偏移
        /// </summary>
        public long Offset { get; private set; }
        /// <summary>
        /// 状态
        /// </summary>
        public DeliveryResultStatus Status { get; private set; }
        /// <summary>
        /// 消息键值
        /// </summary>
        public string Key { get; private set; }
        /// <summary>
        /// 消息
        /// </summary>
        public object Message { get; private set; }
    }
}
