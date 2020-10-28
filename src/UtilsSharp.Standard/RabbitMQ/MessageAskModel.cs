using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ
{
    public class MessageAskModel
    {
        public ulong DeliveryTag { get; set; }

        public string Message { get; set; }
    }
}
