using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace RabbitMQ
{
    /// <summary>
    /// RabbitMq客户端
    /// </summary>
    public sealed class RabbitMqClient
    {
        private static ConcurrentDictionary<string, Tuple<string, string, string>> _cdic;
        private static RabbitMqClient _instance;
        private static readonly object InstanceLock = new object();

        private RabbitMqClient()
        {

        }

        /// <summary>
        ///  RabbitMq实例
        /// </summary>
        public static RabbitMqClient GetInstance()
        {
            switch (_instance)
            {
                case null:
                    {
                        lock (InstanceLock)
                        {
                            if (_instance == null)
                            {
                                _instance = new RabbitMqClient();
                            }
                        }
                        break;
                    }
            }
            return _instance;
        }

        /// <summary>
        /// 生产者（发送消息）
        /// </summary>
        public void Send<T>(string businessName, T content) where T : class
        {
            var rabbitMqHelper = new RabbitMqHelper();
            var t = Get(businessName);
            rabbitMqHelper.Send(t.Item1, t.Item2, content);
        }

        /// <summary>
        /// 生产者（发送消息）
        /// </summary>
        public void Send<T>(string businessName, List<T> contents) where T : class
        {
            var rabbitMqHelper = new RabbitMqHelper();
            var t = Get(businessName);
            rabbitMqHelper.Send(t.Item1, t.Item2, contents);
        }

        /// <summary>
        /// 生产者（发送消息）
        /// </summary>
        public void Send<T>(string businessName, T content, int expiration) where T : class
        {
            var rabbitMqHelper = new RabbitMqHelper();
            var t = Get(businessName);
            rabbitMqHelper.Send(t.Item1, t.Item2, content, expiration);
        }

        /// <summary>
        /// 生产者（发送消息）
        /// </summary>
        public void Send<T>(string businessName, List<T> contents, int expiration) where T : class
        {
            var rabbitMqHelper = new RabbitMqHelper();
            var t = Get(businessName);
            rabbitMqHelper.Send(t.Item1, t.Item2, contents, expiration);
        }

        /// <summary>
        /// 消费者（消费消息）
        /// </summary>
        public void Received(string businessName, Action<string> callback, string exchangeType = "direct", Action errorCallback = null)
        {
            var rabbitMqHelper = new RabbitMqHelper();
            var t = Get(businessName);
            rabbitMqHelper.Received(t.Item1, t.Item2, t.Item3, callback, exchangeType, errorCallback);
        }

        /// <summary>
        /// 批量消费者（消费消息）
        /// </summary>
        public void BatchReceived(string businessName, Action<List<string>> callback, int batchCount = 50, Action errorCallback = null)
        {
            var rabbitMqHelper = new RabbitMqHelper();
            var t = Get(businessName);
            rabbitMqHelper.BatchReceived(t.Item3, callback, batchCount, errorCallback);
        }

        /// <summary>
        /// 获取交换机名、路由名、队列名
        /// </summary>
        /// <param name="businessName">RabbitMq业务名称</param>
        /// <returns></returns>

        private static Tuple<string, string, string> Get(string businessName)
        {
            businessName = string.IsNullOrEmpty(businessName) ? "DefaultBusiness" : businessName.Trim('.');
            if (_cdic == null)
            {
                _cdic = new ConcurrentDictionary<string, Tuple<string, string, string>>();
            }
            if (_cdic.ContainsKey(businessName))
            {
                return _cdic[businessName];
            }
            var exchangeName = $"{businessName}.Exchange";
            var routingKey = $"{businessName}.RoutingKey";
            var queueName = $"{businessName}.Queue";
            var rabbitMqHelper = new RabbitMqHelper();
            //申明交换机
            rabbitMqHelper.ExchangeDeclare(exchangeName);
            //申明队列
            rabbitMqHelper.QueueDeclare(queueName);
            //绑定
            rabbitMqHelper.QueueBind(queueName, exchangeName, routingKey);
            var tuple = new Tuple<string, string, string>(exchangeName, routingKey, queueName);
            _cdic.TryAdd(businessName, tuple);
            return tuple;
        }
    }
}
