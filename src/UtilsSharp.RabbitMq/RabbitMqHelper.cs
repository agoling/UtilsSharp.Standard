using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace UtilsSharp.RabbitMq
{
    /// <summary>
    /// RabbitMqHelper
    /// </summary>
    public abstract class RabbitMqHelper : RabbitMqHelper<RabbitMqHelper> { }

    #region 同步
    /// <summary>
    /// RabbitMq帮助类
    /// </summary>
    public abstract partial class RabbitMqHelper<TMark>
    {
        private static ConcurrentDictionary<string, Tuple<string, string, string>> _cdic = new ConcurrentDictionary<string, Tuple<string, string, string>>();
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
        /// 获取消费者数量
        /// </summary>
        /// <param name="queueName">队列名称</param>
        /// <returns></returns>
        public static uint GetConsumerCount(string queueName)
        {
            return Instance.GetConsumerCount(queueName);
        }

        /// <summary>
        /// 获取消息数量
        /// </summary>
        /// <param name="queueName">队列名称</param>
        /// <returns></returns>
        public static uint GetMessageCount(string queueName)
        {
            return Instance.GetMessageCount(queueName);
        }

        /// <summary>
        /// 申明交换机
        /// </summary>
        /// <param name="exchangeName">交换机名称</param>
        /// <param name="exchangeType">交换机类型</param>
        public static void ExchangeDeclare(string exchangeName, string exchangeType = ExchangeType.Direct)
        {
            Instance.ExchangeDeclare(exchangeName, exchangeType);
        }

        /// <summary>
        /// 删除交换机
        /// </summary>
        /// <param name="exchangeName">交换机名称</param>
        /// <param name="ifUnused">是否要不在使用中</param>
        public static void ExchangeDelete(string exchangeName, bool ifUnused)
        {
            Instance.ExchangeDelete(exchangeName, ifUnused);
        }

        /// <summary>
        /// 申明队列
        /// </summary>
        /// <param name="queueName">队列名称</param>
        /// <param name="arguments">参数</param>
        public static void QueueDeclare(string queueName, IDictionary<string, object> arguments = null)
        {
            Instance.QueueDeclare(queueName, arguments);
        }

        /// <summary>
        /// 删除队列
        /// </summary>
        /// <param name="queueName">队列名称</param>
        public static void QueueDelete(string queueName)
        {
            Instance.QueueDelete(queueName);
        }

        /// <summary>
        /// 清空队列数据
        /// </summary>
        /// <param name="queueName">队列名称</param>
        public static void QueuePurge(string queueName)
        {
            Instance.QueuePurge(queueName);
        }

        /// <summary>
        /// 队列绑定
        /// </summary>
        /// <param name="queueName">队列名称</param>
        /// <param name="exchangeName">交换机名称</param>
        /// <param name="routingKey">路由key</param>
        public static void QueueBind(string queueName, string exchangeName, string routingKey)
        {
            Instance.QueueBind(queueName, exchangeName, routingKey);
        }

        /// <summary>
        /// 队列解绑
        /// </summary>
        /// <param name="queueName">队列名称</param>
        /// <param name="exchangeName">交换机名称</param>
        /// <param name="routingKey">路由key</param>
        public static void QueueUnbind(string queueName, string exchangeName, string routingKey)
        {
            Instance.QueueUnbind(queueName, exchangeName, routingKey);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="exchangeName">交换机名称</param>
        /// <param name="routingKey">路由key</param>
        /// <param name="content">消息内容</param>
        public static void Send<T>(string exchangeName, string routingKey, T content) where T : class
        {
            Instance.Send<T>(exchangeName, routingKey, content);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="exchangeName">交换机名称</param>
        /// <param name="routingKey">路由key</param>
        /// <param name="contents">消息内容集合</param>
        public static void Send<T>(string exchangeName, string routingKey, List<T> contents) where T : class
        {
            Instance.Send<T>(exchangeName, routingKey, contents);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="exchangeName">交换机名称</param>
        /// <param name="routingKey">路由key</param>
        /// <param name="content">消息内容</param>
        /// <param name="expiration">过期时间（秒）</param>
        public static void Send<T>(string exchangeName, string routingKey, T content, int expiration) where T : class
        {
            Instance.Send<T>(exchangeName, routingKey, content, expiration);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="exchangeName">交换机名称</param>
        /// <param name="routingKey">路由key</param>
        /// <param name="contents">消息内容集合</param>
        /// <param name="expiration">过期时间（秒）</param>
        public static void Send<T>(string exchangeName, string routingKey, List<T> contents, int expiration) where T : class
        {
            Instance.Send<T>(exchangeName, routingKey, contents, expiration);
        }

        /// <summary>
        /// 消费者接收消息
        /// </summary>
        /// <param name="queueName">队列名称</param>
        /// <param name="callback">回调方法</param>
        /// <param name="errorCallback">错误回调方法</param>
        public static void Received(string queueName, Action<string> callback,Action<string> errorCallback = null)
        {
            Instance.Received(queueName, callback,errorCallback);
        }

        /// <summary>
        /// 消费者批量接收消息
        /// </summary>
        /// <param name="queueName">队列名称</param>
        /// <param name="callback">回调方法</param>
        /// <param name="batchCount">每次批量接收条数</param>
        /// <param name="errorCallback">错误回调方法</param>
        public static void BatchReceived(string queueName, Action<List<string>> callback, int batchCount = 50, Action<string> errorCallback = null)
        {
            Instance.BatchReceived(queueName, callback, batchCount, errorCallback);
        }

        /// <summary>
        /// 获取消息
        /// </summary>
        /// <param name="queueName">队列名称</param>
        /// <param name="autoAck">是否消息自动确认</param>
        /// <param name="beforeAckAction">手动确认消息前回调(自动确认消息时无效)</param>
        /// <returns></returns>
        public static MessageAskModel GetMessage(string queueName, bool autoAck = true, Action<IModel, MessageAskModel> beforeAckAction = null)
        {
            return Instance.GetMessage(queueName, autoAck, beforeAckAction);
        }

        /// <summary>
        /// 生产者（发送消息）
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="businessName">业务名称</param>
        /// <param name="content">消息内容</param>
        public static void SendByBusiness<T>(string businessName, T content) where T : class
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
        public static void SendByBusiness<T>(string businessName, List<T> contents) where T : class
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
        public static void SendByBusiness<T>(string businessName, T content, int expiration) where T : class
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
        public static void SendByBusiness<T>(string businessName, List<T> contents, int expiration) where T : class
        {
            var t = BindBusiness(businessName);
            Instance.Send(t.Item1, t.Item2, contents, expiration);
        }

        /// <summary>
        /// 消费者（消费消息）
        /// </summary>
        /// <param name="businessName">业务名称</param>
        /// <param name="callback">消费回调方法</param>
        /// <param name="errorCallback">错误回调方法</param>
        public static void ReceivedByBusiness(string businessName, Action<string> callback, Action<string> errorCallback = null)
        {
            var t = BindBusiness(businessName);
            Instance.Received(t.Item3, callback,errorCallback);
        }

        /// <summary>
        /// 批量消费者（消费消息）
        /// </summary>
        /// <param name="businessName">业务名称</param>
        /// <param name="callback">消费回调方法</param>
        /// <param name="batchCount">每次批量接收条数</param>
        /// <param name="errorCallback">错误回调方法</param>
        public static void BatchReceivedByBusiness(string businessName, Action<List<string>> callback, int batchCount = 50, Action<string> errorCallback = null)
        {
            var t = BindBusiness(businessName);
            Instance.BatchReceived(t.Item3, callback, batchCount, errorCallback);
        }

        /// <summary>
        /// 清空队列数据
        /// </summary>
        /// <param name="businessName">业务名称</param>
        public static void QueuePurgeByBusiness(string businessName)
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
        public static MessageAskModel GetMessageByBusiness(string businessName, bool autoAck = true, Action<IModel, MessageAskModel> beforeAckAction = null)
        {
            var t = BindBusiness(businessName);
            var r = Instance.GetMessage(t.Item3, autoAck, beforeAckAction);
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
                _cdic.TryRemove(businessName, out _);
            }
            var exchangeName = $"{businessName}.Exchange";
            var routingKey = $"{businessName}.RoutingKey";
            var queueName = $"{businessName}.Queue";
            //解除绑定
            Instance.QueueUnbind(queueName, exchangeName, routingKey);
            //删除队列
            Instance.QueueDelete(queueName);
            //删除交换机
            Instance.ExchangeDelete(exchangeName, true);
        }
    }
    #endregion

    #region 异步
    /// <summary>
    /// RabbitMq帮助类
    /// </summary>
    public abstract partial class RabbitMqHelper<TMark>
    {
        /// <summary>
        /// 消费者接收消息
        /// </summary>
        /// <param name="queueName">队列名称</param>
        /// <param name="callback">回调方法</param>
        /// <param name="errorCallback">错误回调方法</param>
        public static async Task ReceivedAsync(string queueName, Action<string> callback,Action<string> errorCallback = null)
        {
           await Instance.ReceivedAsync(queueName, callback,errorCallback);
        }

        /// <summary>
        /// 消费者批量接收消息
        /// </summary>
        /// <param name="queueName">队列名称</param>
        /// <param name="callback">回调方法</param>
        /// <param name="batchCount">每次批量接收条数</param>
        /// <param name="errorCallback">错误回调方法</param>
        /// <param name="millisecondsDelay">如果队列无数据休眠时间：毫秒</param>
        public static async Task BatchReceivedAsync(string queueName, Action<List<string>> callback, int batchCount = 50, Action<string> errorCallback = null, int millisecondsDelay = 100)
        {
            await Instance.BatchReceivedAsync(queueName, callback, batchCount, errorCallback, millisecondsDelay);
        }


        /// <summary>
        /// 消费者（消费消息）
        /// </summary>
        /// <param name="businessName">业务名称</param>
        /// <param name="callback">消费回调方法</param>
        /// <param name="errorCallback">错误回调方法</param>
        public static async Task ReceivedByBusinessAsync(string businessName, Action<string> callback,Action<string> errorCallback = null)
        {
            var t = BindBusiness(businessName);
            await Instance.ReceivedAsync(t.Item3, callback,errorCallback);
        }

        /// <summary>
        /// 批量消费者（消费消息）
        /// </summary>
        /// <param name="businessName">业务名称</param>
        /// <param name="callback">消费回调方法</param>
        /// <param name="batchCount">每次批量接收条数</param>
        /// <param name="errorCallback">错误回调方法</param>
        /// <param name="millisecondsDelay">如果队列无数据休眠时间：毫秒</param>
        public static async Task BatchReceivedByBusinessAsync(string businessName, Action<List<string>> callback, int batchCount = 50, Action<string> errorCallback = null, int millisecondsDelay = 100)
        {
            var t = BindBusiness(businessName);
            await Instance.BatchReceivedAsync(t.Item3, callback, batchCount, errorCallback, millisecondsDelay);
        }
    }
    #endregion

}
