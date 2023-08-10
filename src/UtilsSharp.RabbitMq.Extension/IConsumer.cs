using System.Collections.Generic;
using UtilsSharp.Shared.Dependency;

namespace UtilsSharp.RabbitMq.Extension
{
    /// <summary>
    ///  消费者接口
    /// </summary>
    public interface IConsumer : IUnitOfWorkDependency
    {
        /// <summary>
        /// 执行消费者
        /// </summary>
        /// <param name="rabbitMqBusinessName">业务名称</param>
        /// <param name="consumerCount">消费者数量</param>
        /// <param name="consumerHandleCount">每个消费者每次执行条数</param>
        /// <param name="isOnceChannel">是否一次channel</param>
        /// <param name="millisecondsDelay">如果队列无数据休眠时间：毫秒</param>
        void Execute(string rabbitMqBusinessName, int consumerCount = 1, int consumerHandleCount = 500, bool isOnceChannel = false, int millisecondsDelay = 100);


        /// <summary>
        /// 执行消费者
        /// </summary>
        /// <param name="rabbitMqBusinessName">业务名称</param>
        /// <param name="consumerCount">消费者数量</param>
        /// <param name="consumerHandleCount">每个消费者每次执行条数</param>
        /// <param name="isOnceChannel">是否一次channel</param>
        /// <param name="millisecondsDelay">如果队列无数据休眠时间：毫秒</param>
        void ExecuteAsync(string rabbitMqBusinessName, int consumerCount = 1, int consumerHandleCount = 500, bool isOnceChannel = false, int millisecondsDelay = 100);
    }
}
