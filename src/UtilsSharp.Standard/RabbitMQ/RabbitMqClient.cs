using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace RabbitMQ
{
    /// <summary>
    /// RabbitMq客户端
    /// </summary>
    public class RabbitMqClient
    {
        private static ConcurrentDictionary<string, Tuple<string, string, string>> _cdic=new ConcurrentDictionary<string, Tuple<string, string, string>>();
        private static RabbitMqHelper _rabbitMqHelper = new RabbitMqHelper();
        
        private RabbitMqClient()
        {

        }

        /// <summary>
        /// 生产者（发送消息）
        /// </summary>
        public static void Send<T>(string businessName, T content) where T : class
        {
            var t = Get(businessName);
            _rabbitMqHelper.Send(t.Item1, t.Item2, content);
        }

        /// <summary>
        /// 生产者（发送消息）
        /// </summary>
        public static void Send<T>(string businessName, List<T> contents) where T : class
        {
            var t = Get(businessName);
            _rabbitMqHelper.Send(t.Item1, t.Item2, contents);
        }

        /// <summary>
        /// 生产者（发送消息）
        /// </summary>
        public static void Send<T>(string businessName, T content, int expiration) where T : class
        {
            var t = Get(businessName);
            _rabbitMqHelper.Send(t.Item1, t.Item2, content, expiration);
        }

        /// <summary>
        /// 生产者（发送消息）
        /// </summary>
        public static void Send<T>(string businessName, List<T> contents, int expiration) where T : class
        {
            var t = Get(businessName);
            _rabbitMqHelper.Send(t.Item1, t.Item2, contents, expiration);
        }

        /// <summary>
        /// 消费者（消费消息）
        /// </summary>
        public static void Received(string businessName, Action<string> callback, string exchangeType = "direct", Action errorCallback = null)
        {
            var t = Get(businessName);
            _rabbitMqHelper.Received(t.Item1, t.Item2, t.Item3, callback, exchangeType, errorCallback);
        }

        /// <summary>
        /// 批量消费者（消费消息）
        /// </summary>
        public static void BatchReceived(string businessName, Action<List<string>> callback, int batchCount = 50, Action errorCallback = null)
        {
            var t = Get(businessName);
            _rabbitMqHelper.BatchReceived(t.Item3, callback, batchCount, errorCallback);
        }

        /// <summary>
        /// 获取交换机名、路由名、队列名
        /// </summary>
        /// <param name="businessName">RabbitMq业务名称</param>
        /// <returns></returns>

        private static Tuple<string, string, string> Get(string businessName)
        {
            businessName = string.IsNullOrEmpty(businessName) ? "DefaultBusiness" : businessName.Trim('.');
            if (_rabbitMqHelper == null)
            {
                _rabbitMqHelper = new RabbitMqHelper();
            }
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
            //申明交换机
            _rabbitMqHelper.ExchangeDeclare(exchangeName);
            //申明队列
            _rabbitMqHelper.QueueDeclare(queueName);
            //绑定
            _rabbitMqHelper.QueueBind(queueName, exchangeName, routingKey);
            var tuple = new Tuple<string, string, string>(exchangeName, routingKey, queueName);
            _cdic.TryAdd(businessName, tuple);
            return tuple;
        }
    }
}
