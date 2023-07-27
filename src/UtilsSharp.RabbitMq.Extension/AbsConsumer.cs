using System;
using System.Collections.Generic;
using System.Threading;
using UtilsSharp.Logger;

namespace UtilsSharp.RabbitMq.Extension
{
    /// <summary>
    /// 消费者抽象类
    /// </summary>
    public abstract class AbsConsumer<TMark> : IConsumer
    {
        private string _rabbitMqBusinessName = string.Empty;
        /// <summary>
        /// 执行消费者
        /// </summary>
        /// <param name="rabbitMqBusinessName">业务名称</param>
        /// <param name="consumerCount">消费者数量</param>
        /// <param name="consumerHandleCount">每个消费者每次执行条数</param>
        /// <param name="isOnceChannel">是否一次channel</param>
        public virtual void Execute(string rabbitMqBusinessName, int consumerCount = 1, int consumerHandleCount = 500,bool isOnceChannel=false)
        {
            _rabbitMqBusinessName = rabbitMqBusinessName;
            for (var i = 0; i < consumerCount; i++)
            {
                if (isOnceChannel)
                {
                    var thread = new Thread(BatchReceivedOnceChannel) { Name = $"{i}", IsBackground = true };
                    thread.Start(consumerHandleCount);
                }
                else
                {
                    var thread = new Thread(BatchReceived) { Name = $"{i}", IsBackground = true };
                    thread.Start(consumerHandleCount);
                }
            }
            ConsoleHelper.Info($"共开启{consumerCount}个消费者,每个消费者每次拉取{consumerHandleCount}条消息！");
            ConsoleHelper.Info($"数据初始化中,请稍等…");
            ConsoleHelper.Blocking();
        }


        /// <summary>
        /// 消费者
        /// </summary>
        /// <param name="obj">参数</param>
        private void BatchReceived(object obj)
        {
            var consumerHandleCount = (int)obj;
            RabbitMqHelper<TMark>.BatchReceivedByBusiness(_rabbitMqBusinessName, ReceiveCallBack, consumerHandleCount);
        }

        /// <summary>
        /// 消费者
        /// </summary>
        /// <param name="obj">参数</param>
        private void BatchReceivedOnceChannel(object obj)
        {
            var consumerHandleCount = (int)obj;
            RabbitMqHelper<TMark>.BatchReceivedByBusinessOnceChannel(_rabbitMqBusinessName, ReceiveCallBack, consumerHandleCount);
        }

        /// <summary>
        /// 执行消费者
        /// </summary>
        /// <param name="rabbitMqBusinessName">业务名称</param>
        /// <param name="consumerCount">消费者数量</param>
        /// <param name="consumerHandleCount">每个消费者每次执行条数</param>
        /// <param name="isOnceChannel">是否一次channel</param>
        public virtual void ExecuteAsync(string rabbitMqBusinessName, int consumerCount = 1, int consumerHandleCount = 500, bool isOnceChannel = false)
        {
            _rabbitMqBusinessName = rabbitMqBusinessName;
            for (var i = 0; i < consumerCount; i++)
            {
                if (isOnceChannel)
                {
                    var thread = new Thread(BatchReceivedOnceChannelAsync) { Name = $"{i}", IsBackground = true };
                    thread.Start(consumerHandleCount);
                }
                else
                {
                    var thread = new Thread(BatchReceivedAsync) { Name = $"{i}", IsBackground = true };
                    thread.Start(consumerHandleCount);
                }

            }
            ConsoleHelper.Info($"共开启{consumerCount}个消费者,每个消费者每次拉取{consumerHandleCount}条消息！");
            ConsoleHelper.Info($"数据初始化中,请稍等…");
            ConsoleHelper.Blocking();
        }

        /// <summary>
        /// 消费者
        /// </summary>
        /// <param name="obj">参数</param>
        private async void BatchReceivedAsync(object obj)
        {
            var consumerHandleCount = (int)obj;
            await RabbitMqHelper<TMark>.BatchReceivedByBusinessAsync(_rabbitMqBusinessName, ReceiveCallBack, consumerHandleCount);
        }

        /// <summary>
        /// 消费者
        /// </summary>
        /// <param name="obj">参数</param>
        private async void BatchReceivedOnceChannelAsync(object obj)
        {
            var consumerHandleCount = (int)obj;
            await RabbitMqHelper<TMark>.BatchReceivedByBusinessOnceChannelAsync(_rabbitMqBusinessName, ReceiveCallBack, consumerHandleCount);
        }

        /// <summary>
        /// 消费者委托
        /// </summary>
        /// <param name="contents">拉取到的内容</param>
        public abstract void ReceiveCallBack(List<string> contents);
    }
}
