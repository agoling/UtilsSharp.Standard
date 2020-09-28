using OptionConfig;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ
{
    /// <summary>
    /// RabbitMQ帮助类
    /// </summary>
    public class RabbitMQHelper
    {
        IConnection connection = null;
        IModel channel = null;
        public RabbitMQHelper(string rabbitMQConnection)
        {
            connection = new ConnectionFactory() { Uri = new Uri(rabbitMQConnection) }.CreateConnection();
            channel = connection.CreateModel();
        }
        public RabbitMQHelper()
        {
            connection = new ConnectionFactory() { Uri = new Uri(RabbitMQConfig.RabbitMQConnection) }.CreateConnection();
            channel = connection.CreateModel();
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="exchangeName">交换机名称</param>
        /// <param name="routingKey">路由key</param>
        /// <param name="queueName">队列名称</param>
        /// <param name="content">消息内容</param>
        /// <param name="exchangeType">交换机类型，默认：ExchangeType.Direct</param>
        public void Send(string exchangeName, string routingKey, string queueName, string content, string exchangeType=ExchangeType.Direct)
        {
            if (string.IsNullOrEmpty(content)) return;
            channel.ExchangeDeclare(exchangeName, exchangeType, true, false, null);
            channel.QueueDeclare(queueName, true, false, false, null);
            channel.QueueBind(queueName, exchangeName, routingKey, null);
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.DeliveryMode = 1;
            byte[] body = Encoding.UTF8.GetBytes(content);
            //开始发送
            channel.BasicPublish(exchangeName, routingKey, properties, body);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="exchangeName">交换机名称</param>
        /// <param name="routingKey">路由key</param>
        /// <param name="queueName">队列名称</param>
        /// <param name="contents">消息内容集合</param>
        /// <param name="exchangeType">交换机类型，默认：ExchangeType.Direct</param>
        public void Send(string exchangeName, string routingKey, string queueName, List<string> contents, string exchangeType = ExchangeType.Direct)
        {
            if (contents == null || contents.Count == 0) return;
            channel.ExchangeDeclare(exchangeName, exchangeType, true, false, null);
            channel.QueueDeclare(queueName, true, false, false, null);
            channel.QueueBind(queueName, exchangeName, routingKey, null);
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.DeliveryMode = 1;
            foreach(var content in contents)
            {
                byte[] body = Encoding.UTF8.GetBytes(content);
                //开始发送
                channel.BasicPublish(exchangeName, routingKey, properties, body);
            }
        }

        /// <summary>
        /// 创建消费者接收消息
        /// </summary>
        /// <param name="exchangeName">交换机名称</param>
        /// <param name="routingKey">路由key</param>
        /// <param name="queueName">队列名称</param>
        /// <param name="callback">回调函数</param>
        /// <param name="exchangeType">交换机类型，默认：ExchangeType.Direct</param>
        /// <param name="errorCallback">错误回调函数</param>
        public void CreateConsumer(string exchangeName, string routingKey, string queueName, Action<string> callback, string exchangeType = ExchangeType.Direct, Action errorCallback=null)
        {
            Task.Run(() =>
            {
                channel.ExchangeDeclare(exchangeName, exchangeType, true, false, null);
                channel.QueueDeclare(queueName, true, false, false, null);
                channel.QueueBind(queueName, exchangeName, routingKey, null);
                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    ReadOnlyMemory<byte> body = ea.Body;
                    var message = Encoding.UTF8.GetString(body.ToArray());
                    try
                    {
                        callback(message);
                        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    }
                    catch(Exception ex)
                    {
                        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                        errorCallback?.Invoke();
                        return;
                    }
                };
                channel.BasicConsume(queueName, autoAck: false, consumer: consumer);
            });
        }
    }
}
