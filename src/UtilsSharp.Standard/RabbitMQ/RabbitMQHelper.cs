using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using RabbitMQ.Client;

namespace RabbitMQ
{
    /// <summary>
    /// RabbitMqHelper
    /// </summary>
    public abstract class RabbitMqHelper : RabbitMqHelper<RabbitMqHelper> { }

    /// <summary>
    /// RabbitMq帮助类
    /// </summary>
    public abstract class RabbitMqHelper<TMark>
    {
        private static ConcurrentDictionary<string, Tuple<string, string, string>> _cdic=new ConcurrentDictionary<string, Tuple<string, string, string>>();
        private static RabbitMqClient _instance;

        /// <summary>
        /// RabbitMqClient 静态实例，使用前请初始化
        /// RabbitMqHelper.Initialization(new RabbitMqClient())
        /// </summary>
        public static RabbitMqClient Instance
        {
            get
            {
                if (_instance == null) throw new Exception("使用前请初始化 RabbitMqHelper.Initialization(new RabbitMqClient());");
                return _instance;
            }
        }

        /// <summary>
        /// 初始化RabbitMqClient静态访问类
        /// RabbitMqHelper.Initialization(new RabbitMqClient())
        /// </summary>
        /// <param name="rabbitMqClient"></param>
        public static void Initialization(RabbitMqClient rabbitMqClient)
        {
            _instance = rabbitMqClient;
        }

        /// <summary>
        /// 生产者（发送消息）
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="businessName">业务名称</param>
        /// <param name="content">消息内容</param>
        public static void Send<T>(string businessName, T content) where T : class
        {
            var t = BindBusiness(businessName);
            Instance.Send(t.Item1, t.Item2, content);
        }

        /// <summary>
        /// 生产者（发送消息）
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="businessName">业务名称</param>
        /// <param name="contents">消息内容集合</param>
        public static void Send<T>(string businessName, List<T> contents) where T : class
        {
            var t = BindBusiness(businessName);
            Instance.Send(t.Item1, t.Item2, contents);
        }

        /// <summary>
        /// 生产者（发送消息）
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="businessName">业务名称</param>
        /// <param name="content">消息内容集合</param>
        /// <param name="expiration">过期时间（秒）</param>
        public static void Send<T>(string businessName, T content, int expiration) where T : class
        {
            var t = BindBusiness(businessName);
            Instance.Send(t.Item1, t.Item2, content, expiration);
        }

        /// <summary>
        /// 生产者（发送消息）
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="businessName">业务名称</param>
        /// <param name="contents">消息内容集合</param>
        /// <param name="expiration">过期时间（秒）</param>
        public static void Send<T>(string businessName, List<T> contents, int expiration) where T : class
        {
            var t = BindBusiness(businessName);
            Instance.Send(t.Item1, t.Item2, contents, expiration);
        }

        /// <summary>
        /// 消费者（消费消息）
        /// </summary>
        /// <param name="businessName">业务名称</param>
        /// <param name="callback">消费回调方法</param>
        /// <param name="exchangeType">交换机类型，默认：ExchangeType.Direct</param>
        /// <param name="errorCallback">错误回调方法</param>
        public static void Received(string businessName, Action<string> callback, string exchangeType = "direct", Action<string> errorCallback = null)
        {
            var t = BindBusiness(businessName);
            Instance.Received(t.Item1, t.Item2, t.Item3, callback, exchangeType, errorCallback);
        }

        /// <summary>
        /// 批量消费者（消费消息）
        /// </summary>
        /// <param name="businessName">业务名称</param>
        /// <param name="callback">消费回调方法</param>
        /// <param name="batchCount">每次批量接收条数</param>
        /// <param name="errorCallback">错误回调方法</param>
        public static void BatchReceived(string businessName, Action<List<string>> callback, int batchCount = 50, Action<string> errorCallback = null)
        {
            var t = BindBusiness(businessName);
            Instance.BatchReceived(t.Item3, callback, batchCount, errorCallback);
        }

        /// <summary>
        /// 清空队列数据
        /// </summary>
        /// <param name="businessName">业务名称</param>
        public static void QueuePurge(string businessName)
        {
            var t = BindBusiness(businessName);
            Instance.QueuePurge(t.Item3);
        }

        /// <summary>
        /// 获取消息
        /// </summary>
        /// <param name="businessName">业务名称</param>
        /// <param name="autoAck">是否消息自动确认</param>
        /// <param name="beforeAckAction">手动确认消息前回调方法(自动确认消息该回调无效)</param>
        /// <returns></returns>
        public static MessageAskModel GetMessage(string businessName, bool autoAck = true, Action<IModel, MessageAskModel> beforeAckAction = null)
        {
            var t = BindBusiness(businessName);
            var r=Instance.GetMessage(t.Item3, autoAck, beforeAckAction);
            return r;
        }

        /// <summary>
        /// 绑定交换机名、路由名、队列名
        /// </summary>
        /// <param name="businessName">RabbitMq业务名称</param>
        /// <param name="update">是否更新</param>
        /// <returns></returns>
        public static Tuple<string, string, string> BindBusiness(string businessName, bool update = false)
        {
            businessName = string.IsNullOrEmpty(businessName) ? "DefaultBusiness" : businessName.Trim('.');
            if (_cdic == null)
            {
                _cdic = new ConcurrentDictionary<string, Tuple<string, string, string>>();
            }
            if (_cdic.ContainsKey(businessName) && !update)
            {
                return _cdic[businessName];
            }
            var exchangeName = $"{businessName}.Exchange";
            var routingKey = $"{businessName}.RoutingKey";
            var queueName = $"{businessName}.Queue";
            //申明交换机
            Instance.ExchangeDeclare(exchangeName);
            //申明队列
            Instance.QueueDeclare(queueName);
            //绑定
            Instance.QueueBind(queueName, exchangeName, routingKey);
            var tuple = new Tuple<string, string, string>(exchangeName, routingKey, queueName);
            _cdic.TryAdd(businessName, tuple);
            return tuple;
        }

        /// <summary>
        /// 解绑交换机名、路由名、队列名
        /// </summary>
        /// <param name="businessName">RabbitMq业务名称</param>
        /// <returns></returns>
        public static void UnBindBusiness(string businessName)
        {
            businessName = string.IsNullOrEmpty(businessName) ? "DefaultBusiness" : businessName.Trim('.');
            if (_cdic == null)
            {
                _cdic = new ConcurrentDictionary<string, Tuple<string, string, string>>();
            }
            if (_cdic.ContainsKey(businessName))
            {
                _cdic.TryRemove(businessName, out var cdic);
            }
            var exchangeName = $"{businessName}.Exchange";
            var routingKey = $"{businessName}.RoutingKey";
            var queueName = $"{businessName}.Queue";
            //解除绑定
            Instance.QueueUnbind(queueName, exchangeName, routingKey);
            //删除队列
            Instance.QueueDelete(queueName);
            //删除交换机
            Instance.ExchangeDelete(exchangeName,true);
        }
    }
}
