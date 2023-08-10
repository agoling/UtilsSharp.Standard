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
        /// <summary>
        /// 执行消费者
        /// </summary>
        /// <param name="rabbitMqBusinessName">业务名称</param>
        /// <param name="consumerCount">消费者数量</param>
        /// <param name="consumerHandleCount">每个消费者每次执行条数</param>
        /// <param name="isOnceChannel">是否一次channel</param>
        public virtual void Execute(string rabbitMqBusinessName, int consumerCount = 1, int consumerHandleCount = 500,bool isOnceChannel=false)
        {
            var request = new BatchReceivedRequest
            {
                RabbitMqBusinessName = rabbitMqBusinessName,
                ConsumerHandleCount = consumerHandleCount
            };
            for (var i = 0; i < consumerCount; i++)
            {
                if (isOnceChannel)
                {
                    var thread = new Thread(BatchReceivedOnceChannel) { Name = $"{i}", IsBackground = true };
                    thread.Start(request);
                }
                else
                {
                    var thread = new Thread(BatchReceived) { Name = $"{i}", IsBackground = true };
                    thread.Start(request);
                }
            }
            ConsoleHelper.Info($"{rabbitMqBusinessName}:共开启{consumerCount}个消费者,每个消费者每次拉取{consumerHandleCount}条消息！");
            ConsoleHelper.Info($"{rabbitMqBusinessName}:数据初始化中,请稍等…");
            ConsoleHelper.Blocking();
        }


        /// <summary>
        /// 消费者
        /// </summary>
        /// <param name="obj">参数</param>
        private void BatchReceived(object obj)
        {
            var request = (BatchReceivedRequest)obj;
            RabbitMqHelper<TMark>.BatchReceivedByBusiness(request.RabbitMqBusinessName, ReceiveCallBack, request.ConsumerHandleCount, ReceiveErrorCallBack);
        }

        /// <summary>
        /// 消费者
        /// </summary>
        /// <param name="obj">参数</param>
        private void BatchReceivedOnceChannel(object obj)
        {
            var request = (BatchReceivedRequest)obj;
            RabbitMqHelper<TMark>.BatchReceivedByBusinessOnceChannel(request.RabbitMqBusinessName, ReceiveCallBack, request.ConsumerHandleCount, ReceiveErrorCallBack);
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
            var request = new BatchReceivedRequest
            {
                RabbitMqBusinessName = rabbitMqBusinessName,
                ConsumerHandleCount = consumerHandleCount
            };
            for (var i = 0; i < consumerCount; i++)
            {
                if (isOnceChannel)
                {
                    var thread = new Thread(BatchReceivedOnceChannelAsync) { Name = $"{i}", IsBackground = true };
                    thread.Start(request);
                }
                else
                {
                    var thread = new Thread(BatchReceivedAsync) { Name = $"{i}", IsBackground = true };
                    thread.Start(request);
                }
            }
            ConsoleHelper.Info($"{rabbitMqBusinessName}:共开启{consumerCount}个消费者,每个消费者每次拉取{consumerHandleCount}条消息！");
            ConsoleHelper.Info($"{rabbitMqBusinessName}:数据初始化中,请稍等…");
            ConsoleHelper.Blocking();
        }

        /// <summary>
        /// 消费者
        /// </summary>
        /// <param name="obj">参数</param>
        private async void BatchReceivedAsync(object obj)
        {
            var request = (BatchReceivedRequest)obj;
            await RabbitMqHelper<TMark>.BatchReceivedByBusinessAsync(request.RabbitMqBusinessName, ReceiveCallBack, request.ConsumerHandleCount, ReceiveErrorCallBack);
        }

        /// <summary>
        /// 消费者
        /// </summary>
        /// <param name="obj">参数</param>
        private async void BatchReceivedOnceChannelAsync(object obj)
        {
            var request = (BatchReceivedRequest)obj;
            await RabbitMqHelper<TMark>.BatchReceivedByBusinessOnceChannelAsync(request.RabbitMqBusinessName, ReceiveCallBack, request.ConsumerHandleCount, ReceiveErrorCallBack);
        }

        /// <summary>
        /// 消费者委托
        /// </summary>
        /// <param name="contents">拉取到的内容</param>
        public abstract void ReceiveCallBack(List<string> contents);


        /// <summary>
        /// 消费者委托(异常错误捕捉)
        /// </summary>
        /// <param name="exception">异常信息</param>
        public virtual void ReceiveErrorCallBack(string exception)
        {
            ConsoleHelper.Error($"ReceiveErrorCallBack:{exception}");
        }


    }
}
