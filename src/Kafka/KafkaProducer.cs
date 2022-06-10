using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Confluent.Kafka;
using ProducerConfig = Confluent.Kafka.ProducerConfig;
using TopicPartition = Confluent.Kafka.TopicPartition;

namespace Kafka
{
    public class KafkaProducer : IDisposable
    {
        /// <summary>
        /// 负责生成producer
        /// </summary>
        private ProducerBuilder<string, object> _builder;
        private readonly ConcurrentQueue<IProducer<string, object>> _producers;
        private bool _disposed = false;

        /// <summary>
        /// kafka服务节点
        /// </summary>
        public string BootstrapServers { get; }
        /// <summary>
        /// Flush超时时间(ms)
        /// </summary>
        public int FlushTimeOut { get; set; } = 10000;
        /// <summary>
        /// 保留发布者数
        /// </summary>
        public int InitializeCount { get; set; } = 5;
        /// <summary>
        /// 默认的消息键值
        /// </summary>
        public string DefaultKey { get; set; }
        /// <summary>
        /// 默认的主题
        /// </summary>
        public string DefaultTopic { get; set; }
        /// <summary>
        /// 异常事件
        /// </summary>
        public event Action<object, Exception> ErrorHandler;
        /// <summary>
        /// 统计事件
        /// </summary>
        public event Action<object, string> StatisticsHandler;
        /// <summary>
        /// 日志事件
        /// </summary>
        public event Action<object, KafkaLogMessage> LogHandler;

        public KafkaProducer(params string[] bootstrapServers)
        {
            if (bootstrapServers == null || bootstrapServers.Length == 0)
            {
                throw new Exception("at least one server must be assigned");
            }

            this.BootstrapServers = string.Join(",", bootstrapServers);
            _producers = new ConcurrentQueue<Confluent.Kafka.IProducer<string, object>>();
        }

        #region Private
        /// <summary>
        /// producer构造器
        /// </summary>
        /// <returns></returns>
        private void CreateProducerBuilder()
        {
            if (_builder != null) return;
            lock (this)
            {
                if (_builder == null)
                {
                    var config = new ProducerConfig();
                    config.BootstrapServers = BootstrapServers;

                    //var config = new KeyValuePair<string, string>[] { new KeyValuePair<string, string>("bootstrap.servers", BootstrapServers) };

                    _builder = new ProducerBuilder<string, object>(config);
                    Action<Delegate, object> tryCatchWrap = (@delegate, arg) =>
                    {
                        try
                        {
                            @delegate?.DynamicInvoke(arg);
                        }
                        catch { }
                    };
                    _builder.SetErrorHandler((p, e) => tryCatchWrap(ErrorHandler, new Exception(e.Reason)));
                    _builder.SetStatisticsHandler((p, e) => tryCatchWrap(StatisticsHandler, e));
                    _builder.SetLogHandler((p, e) => tryCatchWrap(LogHandler, new KafkaLogMessage(e)));
                    _builder.SetValueSerializer(new KafkaConverter());
                }
            }
        }
        /// <summary>
        /// 租赁一个发布者
        /// </summary>
        /// <returns></returns>
        private IProducer<string, object> RentProducer()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(KafkaProducer));
            }

            IProducer<string, object> producer;
            lock (_producers)
            {
                if (!_producers.TryDequeue(out producer) || producer == null)
                {
                    CreateProducerBuilder();
                    producer = _builder.Build();
                }
            }
            return producer;
        }
        /// <summary>
        /// 返回保存发布者
        /// </summary>
        /// <param name="producer"></param>
        private void ReturnProducer(IProducer<string, object> producer)
        {
            if (_disposed) return;

            lock (_producers)
            {
                if (_producers.Count < InitializeCount && producer != null)
                {
                    _producers.Enqueue(producer);
                }
                else
                {
                    producer?.Dispose();
                }
            }
        }
        #endregion

        #region Send
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="message"></param>
        /// <param name="callback"></param>
        public void SendWithKey(string key, object message, Action<DeliveryResult> callback = null)
        {
            Send(DefaultTopic, null, key, message, callback);
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="partition"></param>
        /// <param name="key"></param>
        /// <param name="message"></param>
        /// <param name="callback"></param>
        public void SendWithKey(int? partition, string key, object message, Action<DeliveryResult> callback = null)
        {
            Send(DefaultTopic, partition, key, message, callback);
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="key"></param>
        /// <param name="message"></param>
        /// <param name="callback"></param>
        public void SendWithKey(string topic, string key, object message, Action<DeliveryResult> callback = null)
        {
            Send(topic, null, key, message, callback);
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="callback"></param>
        public void Send(object message, Action<DeliveryResult> callback = null)
        {
            Send(DefaultTopic, null, DefaultKey, message, callback);
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="partition"></param>
        /// <param name="message"></param>
        /// <param name="callback"></param>
        public void Send(int? partition, object message, Action<DeliveryResult> callback = null)
        {
            Send(DefaultTopic, partition, DefaultKey, message, callback);
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        /// <param name="callback"></param>
        public void Send(string topic, object message, Action<DeliveryResult> callback = null)
        {
            Send(topic, null, DefaultKey, message, callback);
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="partition"></param>
        /// <param name="message"></param>
        /// <param name="callback"></param>
        public void Send(string topic, int? partition, object message, Action<DeliveryResult> callback = null)
        {
            Send(topic, partition, DefaultKey, message, callback);
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="partition"></param>
        /// <param name="key"></param>
        /// <param name="message"></param>
        /// <param name="callback"></param>
        public void Send(string topic, int? partition, string key, object message, Action<DeliveryResult> callback = null)
        {
            Send(new KafkaMessage() { Key = key, Message = message, Partition = partition, Topic = topic }, callback);
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="kafkaMessage"></param>
        /// <param name="callback"></param>
        public void Send(KafkaMessage kafkaMessage, Action<DeliveryResult> callback = null)
        {
            if (string.IsNullOrEmpty(kafkaMessage.Topic))
            {
                throw new ArgumentException("topic can not be empty", nameof(kafkaMessage.Topic));
            }
            if (string.IsNullOrEmpty(kafkaMessage.Key))
            {
                throw new ArgumentException("key can not be empty", nameof(kafkaMessage.Key));
            }

            var producer = RentProducer();
            if (kafkaMessage.Partition == null)
            {
                producer.Produce(kafkaMessage.Topic, new Message<string, object>() { Key = kafkaMessage.Key, Value = kafkaMessage.Message }, dr => callback?.Invoke(new DeliveryResult(dr)));
            }
            else
            {
                var topicPartition = new TopicPartition(kafkaMessage.Topic, new Partition(kafkaMessage.Partition.Value));
                producer.Produce(topicPartition, new Message<string, object>() { Key = kafkaMessage.Key, Value = kafkaMessage.Message }, dr => callback?.Invoke(new DeliveryResult(dr)));
            }
            producer.Flush(TimeSpan.FromMilliseconds(FlushTimeOut));
            ReturnProducer(producer);
        }
        #endregion

        #region SendAsync
        /// <summary>
        /// 异步发送消息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="message"></param>
        public async Task<DeliveryResult> SendWithKeyAsync(string key, object message)
        {
            return await SendAsync(DefaultTopic, null, key, message);
        }
        /// <summary>
        /// 异步发送消息
        /// </summary>
        /// <param name="partition"></param>
        /// <param name="key"></param>
        /// <param name="message"></param>
        public async Task<DeliveryResult> SendWithKeyAsync(int? partition, string key, object message)
        {
            return await SendAsync(DefaultTopic, partition, key, message);
        }
        /// <summary>
        /// 异步发送消息
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="key"></param>
        /// <param name="message"></param>
        public async Task<DeliveryResult> SendWithKeyAsync(string topic, string key, object message)
        {
            return await SendAsync(topic, null, key, message);
        }
        /// <summary>
        /// 异步发送消息
        /// </summary>
        /// <param name="message"></param>
        public async Task<DeliveryResult> SendAsync(object message)
        {
            return await SendAsync(DefaultTopic, null, DefaultKey, message);
        }
        /// <summary>
        /// 异步发送消息
        /// </summary>
        /// <param name="partition"></param>
        /// <param name="message"></param>
        public async Task<DeliveryResult> SendAsync(int? partition, object message)
        {
            return await SendAsync(DefaultTopic, partition, DefaultKey, message);
        }
        /// <summary>
        /// 异步发送消息
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        public async Task<DeliveryResult> SendAsync(string topic, object message)
        {
            return await SendAsync(topic, null, DefaultKey, message);
        }
        /// <summary>
        /// 异步发送消息
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="partition"></param>
        /// <param name="message"></param>
        public async Task<DeliveryResult> SendAsync(string topic, int? partition, object message)
        {
            return await SendAsync(topic, partition, DefaultKey, message);
        }
        /// <summary>
        /// 异步发送消息
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="partition"></param>
        /// <param name="key"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<DeliveryResult> SendAsync(string topic, int? partition, string key, object message)
        {
            return await SendAsync(new KafkaMessage() { Key = key, Message = message, Partition = partition, Topic = topic });
        }
        /// <summary>
        /// 异步发送消息
        /// </summary>
        /// <param name="kafkaMessage"></param>
        /// <returns></returns>
        public async Task<DeliveryResult> SendAsync(KafkaMessage kafkaMessage)
        {
            if (string.IsNullOrEmpty(kafkaMessage.Topic))
            {
                throw new ArgumentException("topic can not be empty", nameof(kafkaMessage.Topic));
            }
            if (string.IsNullOrEmpty(kafkaMessage.Key))
            {
                throw new ArgumentException("key can not be empty", nameof(kafkaMessage.Key));
            }

            var producer = RentProducer();
            DeliveryResult<string, object> deliveryResult;
            if (kafkaMessage.Partition == null)
            {
                deliveryResult = await producer.ProduceAsync(kafkaMessage.Topic, new Message<string, object>() { Key = kafkaMessage.Key, Value = kafkaMessage.Message });
            }
            else
            {
                var topicPartition = new TopicPartition(kafkaMessage.Topic, new Partition(kafkaMessage.Partition.Value));
                deliveryResult = await producer.ProduceAsync(topicPartition, new Message<string, object>() { Key = kafkaMessage.Key, Value = kafkaMessage.Message });
            }

            producer.Flush(new TimeSpan(0, 0, 0, 0, FlushTimeOut));

            ReturnProducer(producer);

            return new DeliveryResult(deliveryResult);
        }

        #endregion

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            _disposed = true;
            while (_producers.Count > 0)
            {
                IProducer<string, object> producer;
                _producers.TryDequeue(out producer);
                producer?.Dispose();
            }
            GC.Collect();
        }
    }
}
