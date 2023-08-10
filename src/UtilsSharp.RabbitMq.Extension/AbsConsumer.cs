using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
        /// <param name="millisecondsDelay">如果队列无数据休眠时间：毫秒</param>
        public virtual void Execute(string rabbitMqBusinessName, int consumerCount = 1, int consumerHandleCount = 500,bool isOnceChannel=false, int millisecondsDelay = 100)
        {
            var request = new BatchReceivedRequest
            {
                RabbitMqBusinessName = rabbitMqBusinessName,
                ConsumerHandleCount = consumerHandleCount,
                IsOnceChannel= isOnceChannel,
                MillisecondsDelay = millisecondsDelay
            };
            for (var i = 0; i < consumerCount; i++)
            {
                var thread = new Thread(BatchReceived) { Name = $"{i}", IsBackground = true };
                thread.Start(request);
            }
            ConsoleHelper.Info($"{rabbitMqBusinessName}:共开启{consumerCount}个消费者,每个消费者每次拉取{consumerHandleCount}条消息！");
            ConsoleHelper.Info($"{rabbitMqBusinessName}:数据初始化中,请稍等…");
            ConsoleHelper.Blocking();
        }


        /// <summary>
        /// 消费者
        /// </summary>
        /// <param name="obj">参数</param>
        protected virtual void BatchReceived(object obj)
        {
            var request = (BatchReceivedRequest)obj;
            if (request.IsOnceChannel)
            {
                RabbitMqHelper<TMark>.BatchReceivedByBusinessOnceChannel(request.RabbitMqBusinessName, ReceiveCallBack, request.ConsumerHandleCount, ReceiveErrorCallBack, request.MillisecondsDelay);
            }
            else
            {
                RabbitMqHelper<TMark>.BatchReceivedByBusiness(request.RabbitMqBusinessName, ReceiveCallBack, request.ConsumerHandleCount, ReceiveErrorCallBack, request.MillisecondsDelay);
            }
        }

        /// <summary>
        /// 执行消费者
        /// </summary>
        /// <param name="rabbitMqBusinessName">业务名称</param>
        /// <param name="consumerCount">消费者数量</param>
        /// <param name="consumerHandleCount">每个消费者每次执行条数</param>
        /// <param name="isOnceChannel">是否一次channel</param>
        /// <param name="millisecondsDelay">如果队列无数据休眠时间：毫秒</param>
        public virtual void ExecuteAsync(string rabbitMqBusinessName, int consumerCount = 1, int consumerHandleCount = 500, bool isOnceChannel = false, int millisecondsDelay = 100)
        {
            var request = new BatchReceivedRequest
            {
                RabbitMqBusinessName = rabbitMqBusinessName,
                ConsumerHandleCount = consumerHandleCount,
                IsOnceChannel = isOnceChannel,
                MillisecondsDelay = millisecondsDelay
            };
            for (var i = 0; i < consumerCount; i++)
            {
                var thread = new Thread(BatchReceivedAsync) { Name = $"{i}", IsBackground = true };
                thread.Start(request);
            }
            ConsoleHelper.Info($"{rabbitMqBusinessName}:共开启{consumerCount}个消费者,每个消费者每次拉取{consumerHandleCount}条消息！");
            ConsoleHelper.Info($"{rabbitMqBusinessName}:数据初始化中,请稍等…");
            ConsoleHelper.Blocking();
        }

        /// <summary>
        /// 消费者
        /// </summary>
        /// <param name="obj">参数</param>
        protected virtual async void BatchReceivedAsync(object obj)
        {
            var request = (BatchReceivedRequest)obj;
            if (request.IsOnceChannel)
            {
                await RabbitMqHelper<TMark>.BatchReceivedByBusinessOnceChannelAsync(request.RabbitMqBusinessName, ReceiveCallBack, request.ConsumerHandleCount, ReceiveErrorCallBack,request.MillisecondsDelay);
            }
            else
            {
                await RabbitMqHelper<TMark>.BatchReceivedByBusinessAsync(request.RabbitMqBusinessName, ReceiveCallBack, request.ConsumerHandleCount, ReceiveErrorCallBack, request.MillisecondsDelay);
            }
        }

        /// <summary>
        /// 消费者委托
        /// </summary>
        /// <param name="contents">拉取到的内容</param>
        protected abstract void ReceiveCallBack(List<string> contents);


        /// <summary>
        /// 消费者委托(异常错误捕捉)
        /// </summary>
        /// <param name="exception">异常信息</param>
        protected virtual void ReceiveErrorCallBack(string exception)
        {
            ConsoleHelper.Error($"ReceiveErrorCallBack:{exception}");
        }

    }
}
