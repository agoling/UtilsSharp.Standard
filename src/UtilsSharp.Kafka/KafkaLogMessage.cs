using System;
using System.Collections.Generic;
using System.Text;
using Confluent.Kafka;

namespace UtilsSharp.Kafka
{
    public class KafkaLogMessage
    {
        internal KafkaLogMessage(LogMessage logMessage)
        {
            this.Name = logMessage.Name;
            this.Facility = logMessage.Facility;
            this.Message = logMessage.Message;

            switch (logMessage.Level)
            {
                case SyslogLevel.Emergency: this.Level = LogLevel.Emergency; break;
                case SyslogLevel.Alert: this.Level = LogLevel.Alert; break;
                case SyslogLevel.Critical: this.Level = LogLevel.Critical; break;
                case SyslogLevel.Error: this.Level = LogLevel.Error; break;
                case SyslogLevel.Warning: this.Level = LogLevel.Warning; break;
                case SyslogLevel.Notice: this.Level = LogLevel.Notice; break;
                case SyslogLevel.Info: this.Level = LogLevel.Info; break;
                case SyslogLevel.Debug: this.Level = LogLevel.Debug; break;
            }
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 级别
        /// </summary>
        public LogLevel Level { get; private set; }
        /// <summary>
        /// 装置
        /// </summary>
        public string Facility { get; private set; }
        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; private set; }

        public enum LogLevel
        {
            Emergency = 0,
            Alert = 1,
            Critical = 2,
            Error = 3,
            Warning = 4,
            Notice = 5,
            Info = 6,
            Debug = 7
        }
    }
}
