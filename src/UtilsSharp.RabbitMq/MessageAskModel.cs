using System;
using System.Collections.Generic;
using System.Text;

namespace UtilsSharp.RabbitMq
{
    /// <summary>
    /// MessageAskModel
    /// </summary>
    public class MessageAskModel
    {
        /// <summary>
        /// DeliveryTag
        /// </summary>
        public ulong DeliveryTag { get; set; }
        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; }
    }
}
